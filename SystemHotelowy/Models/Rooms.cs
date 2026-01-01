using System.Runtime.InteropServices.Marshalling;

namespace SystemHotelowy.Models
{
    public class Rooms
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } 
        public int RoomTypeId { get; set; }
        public virtual RoomType? RoomType { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
    }
}
