using System.ComponentModel.DataAnnotations;

namespace SystemHotelowy.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
