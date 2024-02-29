using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketSale.Domain.Models;
using TicketSale.Infrastructure.Data;

namespace TicketSale.WebUI.Controllers
{
    public class RouteController : Controller
    {
        private readonly TicketSaleDbContext _ticketSaleDbContext;
        public RouteController(TicketSaleDbContext ticketSaleDbContext)
        {
            _ticketSaleDbContext = ticketSaleDbContext;
        }

        // GET: RouteController
        public ActionResult Index()
        {
            var routes= _ticketSaleDbContext.Routes.ToList();
            return View(routes);
        }

        // GET: RouteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RouteController/Create
        public ActionResult Create()
        {
            ViewData["TransportTypeId"] = new SelectList(_ticketSaleDbContext.TransportTypes, "Id", "Name");
            return View();
        }

        // POST: RouteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("PlaceOfDeparture", "PlaceOfDestination","TransportTypeId")] Domain.Models.Route routeToAdd)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _ticketSaleDbContext.Routes.Add(routeToAdd);
                    _ticketSaleDbContext.SaveChanges(); 
                    return RedirectToAction(nameof(Index)); 
                }
                catch (Exception ex)
                {
                   
                    ModelState.AddModelError("", "An error occurred while creating the route.");
                }
            }  
            return View(routeToAdd);
        }



        //// GET: UsersController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    var user = _ticketSaleDbContext.Users.Find(id);
        //    return View(user);
        //}

        //// POST: UsersController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, [Bind("Id,Name,SurName,Email,PhoneNumber,PasswordHash,Role")] User userToUpdate)
        //{
        //    if (id != userToUpdate.Id)
        //    {
        //        return NotFound();
        //    }
        //    try
        //    {
        //        _ticketSaleDbContext.Update(userToUpdate);
        //        _ticketSaleDbContext.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {

        //    }
        //    return View(userToUpdate);
        //}




        // GET: RouteController/Edit/5
        public ActionResult Edit(int id)
        {
            var route = _ticketSaleDbContext.Routes.Find(id);
            return View(route);
        }

        // POST: RouteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("PlaceOfDeparture", "PlaceOfDestination", "TransportTypeId")] Domain.Models.Route routeToUpdate)
        {
            try
            {
                _ticketSaleDbContext.Routes.Update(routeToUpdate);
                _ticketSaleDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
               
            }
            return View();
        }

        // GET: RouteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RouteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
