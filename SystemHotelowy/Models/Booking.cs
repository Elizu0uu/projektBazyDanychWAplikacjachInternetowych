using SystemHotelowy.Areas.Identity.Data;

namespace SystemHotelowy.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public virtual Rooms Rooms { get; set; }
        public int VisitorId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public DateTime StartReservation { get; set; }
        public DateTime EndReservation { get; set; }

    }
}
