using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketSale.Domain;

namespace TicketSale.Infrastructure.Data
{
    public class TicketSaleDbContext : IdentityDbContext<Customer>
    {
        public TicketSaleDbContext(DbContextOptions<TicketSaleDbContext> options):base(options)
        {
            
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingStatus> BookingStatuses { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TransportType> TransportTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketSaleDbContext).Assembly);
        }
    }
}
