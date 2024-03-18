using Microsoft.EntityFrameworkCore;
using TicketSale.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using TicketSale.Domain;
using TicketSale.WebUI.Areas;
using TicketSale.WebUI.TicketSearchService;
using TicketSale.WebUI.Services;


namespace TicketSale.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TicketSaleDbContext>(options => options
           .UseSqlServer(builder.Configuration.GetConnectionString("TicketSaleConnectionString") ?? throw new InvalidOperationException("Connection string 'TicketSaleConnectionString' not found.")));

            //builder.Services.AddDefaultIdentity<Customer>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<TicketSaleDbContext>()
            //    .AddRoles<IdentityRole>();
            builder.Services.AddDefaultIdentity<Customer>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<TicketSaleDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();
        
                
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<SearchService>();
            builder.Services.AddScoped<PurchaseTicketService>();
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var userManager = serviceProvider.GetRequiredService<UserManager<Customer>>();
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    RolesService.InitializeAsync(userManager, roleManager);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}


