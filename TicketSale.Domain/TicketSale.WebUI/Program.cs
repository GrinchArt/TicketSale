using Microsoft.EntityFrameworkCore;
using TicketSale.Infrastructure.Data;

namespace TicketSale.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TicketSaleDbContext>(options => options
            .UseSqlServer(builder.Configuration.GetConnectionString("TicketSaleConnectionString") ?? throw new InvalidOperationException("Connection string 'TicketSaleConnectionString' not found.")));
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
