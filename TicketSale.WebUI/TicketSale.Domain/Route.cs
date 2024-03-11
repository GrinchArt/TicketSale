

namespace TicketSale.Domain
{
    public class Route
    {
        public int Id { get; set; }
        public string CountryOfDeparture {  get; set; }
        public string PlaceOfDeparture { get; set; }
        public string CountryOfDestination { get; set; }
        public string PlaceOfDestination { get; set; }
        public int TransportTypeId { get; set; }
    }
}
