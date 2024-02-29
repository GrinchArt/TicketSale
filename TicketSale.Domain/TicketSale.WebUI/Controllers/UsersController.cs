using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSale.Domain.Models;
using TicketSale.Infrastructure.Data;

namespace TicketSale.WebUI.Controllers
{
    public class UsersController : Controller
    {
        private readonly TicketSaleDbContext _ticketSaleDbContext;

        public UsersController(TicketSaleDbContext ticketSaleDbContext)
        {
            _ticketSaleDbContext = ticketSaleDbContext;
        }

        // GET: UsersController
        public ActionResult Index()
        {
            var users = _ticketSaleDbContext.Users.ToList();
            return View(users);
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name,SurName,Email,PhoneNumber,PasswordHash")] User userToAdd)
        {
            userToAdd.Role = "User";
            _ticketSaleDbContext.Users.Add(userToAdd);
            _ticketSaleDbContext.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            var user = _ticketSaleDbContext.Users.Find(id);
            return View(user);
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Id,Name,SurName,Email,PhoneNumber,PasswordHash,Role")] User userToUpdate)
        {
            if (id != userToUpdate.Id)
            {
                return NotFound();
            }
            try
            {
                _ticketSaleDbContext.Update(userToUpdate);
                _ticketSaleDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {

            }
            return View(userToUpdate);
        }

      
        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
                var userToDelete = _ticketSaleDbContext.Users.Find(id);
                _ticketSaleDbContext.Users.Remove(userToDelete);
                _ticketSaleDbContext.SaveChanges();         
            return RedirectToAction(nameof(Index));
        }
    }
}
