using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SystemHotelowy.Areas.Identity.Data;
using SystemHotelowy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace SystemHotelowy.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<AppUser> _userManager;


        public BookingsController(ApplicationDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            var query = _context.Bookings
              .Include(b => b.Rooms)
              .Include(b => b.Status)
              .Include(b => b.AppUser)
              .AsQueryable();

            if (!User.IsInRole("Admin") && !User.IsInRole("Receptionist"))
            {
                query = query.Where(b => b.VisitorId == currentUserId);
            }
            var bookings = await query.ToListAsync();
            return View(bookings);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
              .Include(b => b.Status)
              .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create(DateTime? start, DateTime? end, int? capacity)
        {
            var allRooms = _context.Rooms.AsQueryable();
            if (capacity.HasValue)
            {
                allRooms = allRooms.Where(r => r.Capacity >= capacity.Value);
            }
            if (start.HasValue && end.HasValue)
            {
                var occupiedRoomIds = _context.Bookings
                  .Where(b => b.StatusId != 4 &&
                     ((start < b.EndReservation) && (end > b.StartReservation)))
                  .Select(b => b.RoomId)
                  .ToList();

                allRooms = allRooms.Where(r => !occupiedRoomIds.Contains(r.Id));
            }

            ViewData["RoomId"] = new SelectList(allRooms.Select(r => new
            {
                Id = r.Id,
                Display = $"Room {r.RoomNumber} (Capacity: {r.Capacity})"
            }), "Id", "Display");
            ViewData["VisitorId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["StatusId"] = new SelectList(_context.Statutes, "Id", "Name");

            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomId,VisitorId,StartReservation,EndReservation,TotalPrice,StatusId")] Booking booking)
        {
            ModelState.Remove("Rooms");
            ModelState.Remove("AppUser");
            ModelState.Remove("Status");

            if (!User.IsInRole("Receptionist") && !User.IsInRole("Admin"))
            {
                booking.VisitorId = _userManager.GetUserId(User);
                ModelState.Remove("VisitorId");
            }

            booking.StatusId = User.IsInRole("Receptionist") ? 2 : 1;

            if (booking.EndReservation <= booking.StartReservation)
            {
                ModelState.AddModelError("", "Final booking should be made after the start date");
                ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNumber", booking.RoomId);
                return View(booking);
            }

            bool isOccupied = await _context.Bookings.AnyAsync(b => b.RoomId == booking.RoomId && b.StatusId != 4 &&
            booking.StartReservation < b.EndReservation && booking.EndReservation > b.StartReservation);

            if (isOccupied)
            {
                ModelState.AddModelError("", "This room will be occupied on this date");
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
            }
            if (ModelState.IsValid)
            {
                var room = await _context.Rooms.FindAsync(booking.RoomId);

                if (room != null)
                {
                    var days = (booking.EndReservation - booking.StartReservation).Days;
                    booking.TotalPrice = (days > 0 ? days : 1) * room.PricePerNight;

                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Reservation has been successfully created!";
                    return RedirectToAction(nameof(Details), new { id = booking.Id });
                }

                ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNumber", booking.RoomId);
                return View(booking);

            }
            ViewData["RoomId"] = new SelectList(_context.Rooms.Select(r => new
            {
                Id = r.Id,
                Display = $"Room {r.RoomNumber} (Capacity: {r.Capacity})"
            }), "Id", "Display", booking.RoomId);
            ViewData["VisitorId"] = new SelectList(_context.Users, "Id", "Email", booking.VisitorId);

            return View(booking);
        }

        // GET: Bookings/Edit/5
        [Authorize(Roles = "Receptionist")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["StatusId"] = new SelectList(_context.Statutes, "Id", "Id", booking.StatusId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Receptionist")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomId,VisitorId,StartReservation,EndReservation,TotalPrice,StatusId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusId"] = new SelectList(_context.Statutes, "Id", "Id", booking.StatusId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
              .Include(b => b.Status)
              .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}