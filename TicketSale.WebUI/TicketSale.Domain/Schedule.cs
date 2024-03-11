

namespace TicketSale.Domain
{
    public class Schedule
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int AvailableSeats { get; set; }
    }
}
