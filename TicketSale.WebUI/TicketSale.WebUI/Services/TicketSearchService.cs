using Microsoft.EntityFrameworkCore;
using System.Linq;
using TicketSale.Domain;
using TicketSale.Infrastructure.Data;
using TicketSale.WebUI.Models;

namespace TicketSale.WebUI.TicketSearchService
{
    public class SearchService
    {
        private readonly TicketSaleDbContext _ticketSaleDbContext;

        public SearchService(TicketSaleDbContext ticketSaleDbContext)
        {
            _ticketSaleDbContext = ticketSaleDbContext;
        }

        public IEnumerable<RouteScheduleViewModel> SearchRoutes(SearchCriteria criteria)
        {
 
            var routes = _ticketSaleDbContext.Routes
                .Where(r => r.PlaceOfDeparture == criteria.From && r.PlaceOfDestination == criteria.To)
                .ToList();


            var schedules = _ticketSaleDbContext.Schedules
                .Where(s => routes.Select(r => r.Id).Contains(s.RouteId))
                .ToList();

            // Формуємо вихідні дані
            var result = routes.Select(route => new RouteScheduleViewModel
            {
                RouteInfo = route,
                Schedules = schedules.Where(s => s.RouteId == route.Id).ToList()
            });

            return result;
        }
        public IEnumerable<RouteScheduleViewModel> SearchRoutesAfterDate(SearchCriteria criteria)
        {
            var routes = _ticketSaleDbContext.Routes
                .Where(r => r.PlaceOfDeparture == criteria.From && (criteria.To == null || r.PlaceOfDestination == criteria.To))
                .ToList();

            var schedules = _ticketSaleDbContext.Schedules
                .Where(s => routes.Select(r => r.Id).Contains(s.RouteId)
                        && s.DepartureTime > (criteria.DepartureDate ?? DateTime.Now))
                .OrderBy(s => s.DepartureTime)
                .ToList();

            var result = routes.Join(schedules,
                route => route.Id,
                schedule => schedule.RouteId,
                (route, schedule) => new RouteScheduleViewModel
                {
                    RouteInfo = route,
                    Schedules = new List<Schedule> { schedule }
                });

            return result;
        }
    }
}


