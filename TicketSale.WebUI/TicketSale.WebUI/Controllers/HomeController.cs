using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TicketSale.Domain;
using TicketSale.Infrastructure.Data;
using TicketSale.WebUI.Models;
using TicketSale.WebUI.Services;
using TicketSale.WebUI.TicketSearchService;

namespace TicketSale.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TicketSaleDbContext _ticketSaleDbContext;
        private readonly SearchService _searchService;
        private readonly PurchaseTicketService _purchaseService;
        private readonly UserManager<Customer> _userManager;

        public HomeController(ILogger<HomeController> logger, TicketSaleDbContext ticketSaleDbContext, SearchService searchService, UserManager<Customer> userManager, PurchaseTicketService purchaseService)
        {
            _logger = logger;
            _ticketSaleDbContext = ticketSaleDbContext;
            _searchService = searchService;
            _userManager = userManager;
            _purchaseService= purchaseService;
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
        public IActionResult Search(SearchCriteria criteria, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(criteria.From))
            {
                ModelState.AddModelError("From", "Please enter a departure place.");
            }
            if (string.IsNullOrWhiteSpace(criteria.To))
            {
                ModelState.AddModelError("To", "Please enter an arrival place.");
            }
            var criteriaToSearch = new SearchCriteria
            {
                From = criteria.From,
                To = criteria.To,
                DepartureDate = criteria.DepartureDate,
                ReturnDate = criteria.ReturnDate
            };
            const int pageSize = 4;
            var searchResult = _searchService.SearchRoutes(criteria);
            var pagedResult = searchResult.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            if (!searchResult.Any() && criteria.DepartureDate.HasValue)
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


        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async  Task<IActionResult> Purchase(int scheduleId, int price)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Random rand = new Random();
            var model = new PurchaseTicketViewModel
            {
                ScheduleId = scheduleId,
                FirstName = user.FirstName, 
                LastName = user.LastName,
                Email = user.Email,
                Price = price
            };

            return View(model);
        }


        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> Purchase(PurchaseTicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            decimal finalPrice = model.Price;
            //if (model.IsWithPet) finalPrice += 50;
            //if (model.IsWithBaggage) finalPrice += 50;
            //if (!model.IsOneWay) finalPrice *= 1.7m;
            var s = await _purchaseService.PurchaseTicket(model);
            _ticketSaleDbContext.Add(s);
            _ticketSaleDbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
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
