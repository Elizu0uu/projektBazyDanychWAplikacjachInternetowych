using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemHotelowy.Areas.Identity.Data;
using SystemHotelowy.Models;

namespace SystemHotelowy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public RoomsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms
                              .Include(r => r.RoomType)
                              .ToListAsync();
            return View(await _context.Rooms.ToListAsync());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rooms = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rooms == null)
            {
                return NotFound();
            }

            return View(rooms);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            var typesFromDb = _context.RoomTypes.ToList();
            ViewBag.RoomTypeId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(typesFromDb, "Id", "Name");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomNumber,RoomTypeId,Capacity,PricePerNight,Description,IsAvailable")] Rooms rooms)
        {
            ModelState.Remove("RoomType");
            ModelState.Remove("Bookings");

            if (ModelState.IsValid)
            {
                _context.Add(rooms);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "The room was successfully added!";
                return RedirectToAction(nameof(Create));
            }
            ViewBag.RoomTypeId = new SelectList(_context.RoomTypes, "Id", "Name", rooms.RoomTypeId);
            return View(rooms);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rooms = await _context.Rooms.FindAsync(id);
            if (rooms == null)
            {
                return NotFound();
            }
            ViewBag.RoomTypeId = new SelectList(_context.RoomTypes, "Id", "Name", rooms.RoomTypeId);
            return View(rooms);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomNumber,RoomTypeId,Capacity,PricePerNight,Description,IsAvailable")] Rooms rooms)
        {
            if (id != rooms.Id)
            {
                return NotFound();
            }

            ModelState.Remove("RoomType");
            ModelState.Remove("Bookings");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rooms);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "The Changes have been saved!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomsExists(rooms.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.RoomTypeId = new SelectList(_context.RoomTypes, "Id", "Name", rooms.RoomTypeId);
                return View(rooms);
            }
            return View(rooms);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rooms = await _context.Rooms
                .Include(b => b.RoomType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rooms == null)
            {
                return NotFound();
            }

            return View(rooms);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rooms = await _context.Rooms.FindAsync(id);
            if (rooms != null)
            {
                _context.Rooms.Remove(rooms);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomsExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
