namespace SystemHotelowy.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
