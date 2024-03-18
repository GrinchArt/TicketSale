namespace TicketSale.WebUI.Models
{
    public class PurchaseTicketViewModel
    {
        public bool IsOneWay { get; set; }
        public bool IsWithPet { get; set; }
        public bool IsWithBaggage { get; set; }
        public int ScheduleId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public decimal Price { get; set; }
    }
}
