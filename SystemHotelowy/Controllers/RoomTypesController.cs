using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemHotelowy.Areas.Identity.Data;
using SystemHotelowy.Models;

namespace SystemHotelowy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomTypesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public RoomTypesController(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.RoomTypes.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] RoomType roomType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomType);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Nowy typ pokoju został dodany!";

                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null) return NotFound();

            return View(roomType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] RoomType roomType)
        {
            if (id != roomType.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomType);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Typ pokoju został zaktualizowany.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomTypeExists(roomType.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var roomType = await _context.RoomTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (roomType == null) return NotFound();

            return View(roomType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType != null)
            {
                // UWAGA: SQL rzuci błąd, jeśli spróbujesz usunąć typ, do którego są przypisane pokoje!
                _context.RoomTypes.Remove(roomType);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Typ pokoju został usunięty.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RoomTypeExists(int id)
        {
            return _context.RoomTypes.Any(e => e.Id == id);
        }
    }
}