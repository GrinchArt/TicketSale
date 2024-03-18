using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketSale.Domain;
using TicketSale.Infrastructure.Data;
using TicketSale.WebUI.Models;

namespace TicketSale.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly TicketSaleDbContext _ticketSaleDbContext;
        private readonly UserManager<Customer> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(TicketSaleDbContext ticketSaleDbContext, UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager)
        {
            _ticketSaleDbContext = ticketSaleDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> MainAdminPage()
        {
            var viewAdmin = new AdminMainPageView
            {
                Schedules = _ticketSaleDbContext.Schedules.ToList(),
                Routes = _ticketSaleDbContext.Routes.ToList()
            };
            

            return View(viewAdmin);
        }


        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList()
        {
            if (_userManager == null)
            {
                throw new InvalidOperationException("UserManager is not initialized.");
            }
            var users = _userManager.Users.ToList();
            if (users == null)
            {
                return NotFound("No users found.");
            }
            var userRolesViewModel = new List<UserRolesViewModel>();


            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRolesViewModel.Add(new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = roles
                });
            }

            return View(userRolesViewModel);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                }).ToList(),
                SelectedRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserRole(EditUserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, roles);
            }

            var result = await _userManager.AddToRoleAsync(user, model.SelectedRole);
            if (result.Succeeded)
            {
                return RedirectToAction("UserList");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            model.Roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();

            return View(model);
        }


        //Route Section
        //[Authorize(Roles = "Admin")]
        public async  Task<IActionResult> CreateRoute()
        {
            ViewData["TransportTypeId"] = new SelectList(_ticketSaleDbContext.TransportTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoute([Bind("CountryOfDeparture","PlaceOfDeparture", "CountryOfDestination", "PlaceOfDestination", "TransportTypeId")] Domain.Route routeToAdd)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _ticketSaleDbContext.Routes.Add(routeToAdd);
                    _ticketSaleDbContext.SaveChanges();
                    return RedirectToAction("MainAdminPage");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the route.");
                }
            }
            return View(routeToAdd);
        }

        public ActionResult EditRoute(int id)
        {
            ViewData["TransportTypeId"] = new SelectList(_ticketSaleDbContext.TransportTypes, "Id", "Name");
            var route = _ticketSaleDbContext.Routes.Find(id);
            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRoute(int id, [Bind("CountryOfDeparture", "PlaceOfDeparture", "CountryOfDestination", "PlaceOfDestination", "TransportTypeId")] Domain.Route routeToUpdate)
        {
            try
            {
                _ticketSaleDbContext.Routes.Update(routeToUpdate);
                _ticketSaleDbContext.SaveChanges();
                return RedirectToAction("MainAdminPage");
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while creating the Route.");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoute(int id, IFormCollection collection)
        {
            try
            {
                var routeToDelete = _ticketSaleDbContext.Routes.Find(id);
                if (routeToDelete == null)
                {
                    return NotFound();
                }

                _ticketSaleDbContext.Routes.Remove(routeToDelete);
                _ticketSaleDbContext.SaveChanges();

                return RedirectToAction("MainAdminPage");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }



        //Schedule Section
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSchedule()
        {

            var routes = _ticketSaleDbContext.Routes.ToList();
            ViewData["Routes"] = routes.Select(x => new SelectListItem
            {
                Text = $"{x.PlaceOfDeparture+" "+x.CountryOfDeparture} - {x.PlaceOfDestination + " " + x.CountryOfDestination}",
                Value = x.Id.ToString()
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateSchedule([Bind("RouteId", "DepartureTime", "ArrivalTime", "AvailableSeats")] Schedule scheduleToAdd)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _ticketSaleDbContext.Schedules.Add(scheduleToAdd);
                    _ticketSaleDbContext.SaveChanges();
                    return RedirectToAction("MainAdminPage");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "An error occurred while creating the schedule.");
                }
            }
            return View(scheduleToAdd);
        }


        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditSchedule(int id)
        {
            var schedule = _ticketSaleDbContext.Schedules.Find(id);
            if (schedule == null)
            {
                return NotFound();
            }
            var routes = _ticketSaleDbContext.Routes.ToList();
            ViewData["Routes"] = routes.Select(x => new SelectListItem
            {
                Text = $"{x.PlaceOfDeparture + " " + x.CountryOfDeparture} - {x.PlaceOfDestination + " " + x.CountryOfDestination}",
                Value = x.Id.ToString()
            }).ToList();
            return View(schedule);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, [Bind("Id,RouteId,DepartureTime,ArrivalTime,AvailableSeats")] Schedule scheduleToUpdate)
        {
            try
            {
                _ticketSaleDbContext.Schedules.Update(scheduleToUpdate);
                _ticketSaleDbContext.SaveChanges();
                return RedirectToAction("MainAdminPage");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSchedule(int id, IFormCollection collection)
        {
            try
            {
                var scheduleToDelete = _ticketSaleDbContext.Schedules.Find(id);
                if (scheduleToDelete == null)
                {
                    return NotFound();
                }

                _ticketSaleDbContext.Schedules.Remove(scheduleToDelete);
                _ticketSaleDbContext.SaveChanges();

                return RedirectToAction("MainAdminPage");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
