using TicketSale.Domain;

namespace TicketSale.WebUI.Models
{
    public class AdminMainPageView
    {
        public List<Domain.Route> Routes{ get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}
