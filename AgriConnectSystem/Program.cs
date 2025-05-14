using AgriConnectSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AgriConnectSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ? Add DbContext with SQL Server support
            builder.Services.AddDbContext<AgriConnectContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ? Add Session Services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Optional: sets timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // ? Add MVC + HttpContext Accessor
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // ? Enable Session Middleware
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
