using TicketSale.Domain;

namespace TicketSale.WebUI.Models
{
    public class RouteScheduleViewModel
    {
        public Domain.Route RouteInfo { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}
