using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TicketSale.Infrastructure.Data;
using TicketSale.WebUI.Models;
using TicketSale.WebUI.TicketSearchService;

namespace TicketSale.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TicketSaleDbContext _ticketSaleDbContext;
        private readonly SearchService _searchService;


        public HomeController(ILogger<HomeController> logger, TicketSaleDbContext ticketSaleDbContext, SearchService searchService)
        {
            _logger = logger;
            _ticketSaleDbContext = ticketSaleDbContext;
            _searchService = searchService;
 
        }
       
        public IActionResult Index()
        {
           var routes = _ticketSaleDbContext.Routes.ToList();
            var schedules = _ticketSaleDbContext.Schedules.ToList();
            var viewModel = routes.Select(route => new RouteScheduleViewModel
            {
                RouteInfo = route,
                Schedules = schedules.Where(s => s.RouteId == route.Id).ToList()
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Search(string from, string to, DateTime? departureDate, DateTime? returnDate, int page = 1)
        {
            var criteria = new SearchCriteria
            {
                From = from,
                To = to,
                DepartureDate = departureDate,
                ReturnDate = returnDate
            };
            const int pageSize = 6;
            var searchResult = _searchService.SearchRoutes(criteria);
            var pagedResult = searchResult.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            if (!searchResult.Any() && departureDate.HasValue)
            {
                searchResult = _searchService.SearchRoutesAfterDate(criteria);
            }

            var viewModel = new SearchResultViewModel
            {
                RouteSchedules = pagedResult.ToList(),
                Criteria = criteria,
                CurrentPage = page,
                PageCount = (int)Math.Ceiling((double)searchResult.Count() / pageSize)
            };

            return View("SearchResult", viewModel);
        }

        [HttpGet]
        public IActionResult GetDestinations(string from)
        {
            var destinations = _ticketSaleDbContext.Routes
                                  .Where(r => r.PlaceOfDeparture == from)
                                  .Select(r => r.PlaceOfDestination)
                                  .Distinct()
                                  .ToList();

            return Ok(destinations);
        }



        //public IActionResult PopularRoutes(int page = 1)
        //{
        //    const int pageSize = 6;
        //    var routesQuery = _ticketSaleDbContext.Routes.AsQueryable(); 

        //    var pagedRoutes = routesQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //    var totalRoutes = routesQuery.Count();

        //    var viewModel = new PopularRoutesViewModel
        //    {
        //        Routes = pagedRoutes,
        //        CurrentPage = page,
        //        TotalPages = (int)Math.Ceiling(totalRoutes / (double)pageSize)
        //    };

        //    return PartialView("_PopularRoutes", viewModel);
        //}





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
