using Microsoft.AspNetCore.Identity;


namespace TicketSale.Domain
{
    public class Customer : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
