using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SystemHotelowy.Areas.Identity.Data;

namespace SystemHotelowy.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Rooms? Rooms { get; set; }
        public string? VisitorId { get; set; }
        [ForeignKey("VisitorId")]
        public virtual AppUser? AppUser { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartReservation { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndReservation { get; set; }
        public decimal TotalPrice { get; set; }
        public int StatusId { get; set;}
        public virtual Status? Status { get; set; } 
        public string DisplayStatus
        {
            get
            {
                var now = DateTime.Now;
                if((StatusId == 1 || StatusId == 2) && now > StartReservation.AddDays(1))
                {
                    return "No-show";
                }
                if(StatusId == 3 && now > EndReservation)
                {
                    return "Pending Checkout";
                }
                return Status?.Name ?? "Unknown";
            }
        }

    }
}
