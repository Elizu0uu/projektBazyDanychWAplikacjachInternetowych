using SystemHotelowy.Areas.Identity.Data;

namespace SystemHotelowy.Models
{
    public class UserRolesViewModel
    {
        public AppUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
