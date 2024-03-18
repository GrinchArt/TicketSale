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

            var routesQuery = _ticketSaleDbContext.Routes.AsQueryable();

            // Basic filters
            if (!string.IsNullOrWhiteSpace(criteria.From))
            {
                routesQuery = routesQuery.Where(r => r.PlaceOfDeparture == criteria.From);
            }
            if (!string.IsNullOrWhiteSpace(criteria.To))
            {
                routesQuery = routesQuery.Where(r => r.PlaceOfDestination == criteria.To);
            }

            var routes = routesQuery.ToList();
            var schedulesQuery = _ticketSaleDbContext.Schedules.AsQueryable();

            // Date filters
            if (criteria.DepartureDate.HasValue)
            {
                schedulesQuery = schedulesQuery.Where(s => s.DepartureTime >= criteria.DepartureDate.Value);
            }
            if (criteria.ReturnDate.HasValue)
            {
                schedulesQuery = schedulesQuery.Where(s => s.ArrivalTime <= criteria.ReturnDate.Value);
            }

            var schedules = schedulesQuery.ToList();

            //Output result
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


