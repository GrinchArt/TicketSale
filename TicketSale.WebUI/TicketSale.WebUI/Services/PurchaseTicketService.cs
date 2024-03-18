using Microsoft.AspNetCore.Identity;
using TicketSale.Domain;
using TicketSale.Infrastructure.Data;
using TicketSale.WebUI.Models;

namespace TicketSale.WebUI.Services
{
    public class PurchaseTicketService
    {
        private readonly TicketSaleDbContext _ticketSaleDbContext;
        private readonly UserManager<Customer> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PurchaseTicketService(TicketSaleDbContext ticketSaleDbContext, UserManager<Customer> userManager, IHttpContextAccessor httpContextAccessor) 
        {
            _ticketSaleDbContext = ticketSaleDbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<Ticket> PurchaseTicket(PurchaseTicketViewModel model)
        {
            var customer = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if(customer == null)
            {
                throw new Exception("User is not logged in.");
            }     
            var ticketToBuy = new Ticket 
            { 
                Price = model.Price,
                IsOneWay = model.IsOneWay,
                IsWithPet = model.IsWithPet,
                IsWithBaggage = model.IsWithBaggage,
                SeatNumber = GenerateRandomSeatNumber(55),
                ScheduleId = model.ScheduleId,
                CustomerId = customer.Id
            };
            return ticketToBuy;
        }
      
        private string GenerateRandomSeatNumber(int availableSeats)
        {
            Random rand = new Random();
            int seatNumber = rand.Next(1, availableSeats + 1);
            string seatLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string randomLetters = new string(Enumerable.Repeat(seatLetters, 2)
                                                        .Select(s => s[rand.Next(s.Length)]).ToArray());
            return $"{seatNumber}{randomLetters}";
        }


        public async Task<Ticket> ReturnPayment(int id)
        {
            var ticket = await _ticketSaleDbContext.Tickets.FindAsync(id);
            if (ticket == null)
            {
                throw new Exception("Ticket not found.");
            }

            _ticketSaleDbContext.Tickets.Remove(ticket);
            await _ticketSaleDbContext.SaveChangesAsync();

            return ticket;
        }
    }
}
