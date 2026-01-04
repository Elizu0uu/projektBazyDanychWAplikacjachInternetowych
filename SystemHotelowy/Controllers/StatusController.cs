using Microsoft.AspNetCore.Mvc;

namespace SystemHotelowy.Controllers
{
    public class StatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
