
namespace TicketSale.Domain
{
    public class Ticket
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public bool IsOneWay { get; set; }
        public bool IsWithPet { get; set; }
        public bool IsWithBaggage { get; set; }
        public string SeatNumber { get; set; }

        public int ScheduleId { get; set; }
        public string CustomerId { get; set; }
    }
}
