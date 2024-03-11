namespace TicketSale.WebUI.Models
{
    public class SearchCriteria
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }

    }
}
