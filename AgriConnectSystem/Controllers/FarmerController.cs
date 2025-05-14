using AgriConnectSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AgriConnectSystem.Controllers
{
    public class FarmerController : Controller
    {
        private readonly AgriConnectContext _context;

        public FarmerController(AgriConnectContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Employee")
            {
                return Unauthorized();
            }

            return View(_context.Farmers.Include(f => f.AppUser).ToList());
        }

        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Employee")
            {
                return Unauthorized();
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(string username, string password, string fullName, string email)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Employee")
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }

            if (_context.AppUsers.Any(u => u.Username == username))
            {
                ViewBag.Error = "Username already exists.";
                return View();
            }

            var appUser = new AppUser
            {
                Username = username,
                Password = password,
                Role = "Farmer"
            };

            _context.AppUsers.Add(appUser);
            _context.SaveChanges();

            var farmer = new Farmer
            {
                AppUserId = appUser.Id,
                FullName = fullName,
                Email = email,
                PhoneNumber = "N/A" // Default phone to prevent null constraint violation
            };

            _context.Farmers.Add(farmer);
            _context.SaveChanges();

            TempData["Message"] = "Farmer successfully created.";
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var farmer = _context.Farmers
                .Include(f => f.Products)
                .Include(f => f.AppUser)
                .FirstOrDefault(f => f.Id == id);

            return farmer == null ? NotFound() : View(farmer);
        }
    }
}
