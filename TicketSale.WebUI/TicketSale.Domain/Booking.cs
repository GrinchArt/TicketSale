namespace TicketSale.Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int BookingStatusId { get; set; }
    }
}
