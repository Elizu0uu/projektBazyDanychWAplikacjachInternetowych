using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemHotelowy.Areas.Identity.Data;

namespace SystemHotelowy.Controllers
{
    [Authorize(Roles = "Receptionist")]
    public class ReceptionController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ReceptionController(ApplicationDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string tab = "pending")
        {
            var allBookings = _context.Bookings.AsQueryable();

            ViewBag.CountPending = await allBookings.CountAsync(b => b.StatusId == 1);
            ViewBag.CountConfirmed = await allBookings.CountAsync(b => b.StatusId == 2 && b.StartReservation == DateTime.Today);
            ViewBag.CountCheckIn = await allBookings.CountAsync(b => b.StatusId == 3 && b.EndReservation == DateTime.Today);

            var query = _context.Bookings
                .Include(b => b.Rooms)
                .Include(b => b.Visitor)
                .Include(b => b.Status)
                .AsQueryable();
            switch(tab)
            {
                case "confirmed":
                    query = query.Where(b => b.StatusId == 2);
                    break;
                case "checkout":
                    query = query.Where(b => b.StatusId == 3);
                    break;
                case "archived":
                    query =query.Where(b => b.StatusId == 4 ||  b.StatusId == 5);
                    break;
                default:
                    query = query.Where(b => b.StatusId == 1);
                    tab = "pending";
                    break;
            }
            var result = await query
                .OrderBy(b => b.StartReservation)
                .ThenBy(b => b.Visitor.LastName)
                .ToListAsync();

            ViewBag.ActiveTab = tab;
            return View(result);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
              .Include(b => b.Status)
              .Include(b => b.Rooms)
              .Include(b => b.Visitor)
              .Include(b => b.Receptionist)
              .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, int newStatus, string currentTab)
        {
            var booking = await _context.Bookings.FindAsync(id);
            var currentUserId = _userManager.GetUserId(User);
            booking.ReceptionistId = currentUserId;

            if (booking != null)
            {
                booking.StatusId = newStatus;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Status updated successfully";
            }
            return RedirectToAction(nameof(Index), new {tab= currentTab});
        }
    }
}
