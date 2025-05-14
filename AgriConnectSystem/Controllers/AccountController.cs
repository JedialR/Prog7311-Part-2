using AgriConnectSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AgriConnectSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AgriConnectContext _context;

        public AccountController(AgriConnectContext context)
        {
            _context = context;
        }

        // ------------------- LOGIN -------------------
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    ViewBag.Error = "Please enter both username and password.";
                    return View();
                }

                var user = _context.AppUsers
                    .FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user == null)
                {
                    ViewBag.Error = "Incorrect username or password.";
                    return View();
                }

                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserRole", user.Role);

                return user.Role.ToLower() switch
                {
                    "farmer" => RedirectToAction("Index", "Product"),
                    "employee" => RedirectToAction("EmployeeDashboard", "Home"),
                    _ => RedirectToAction("Index", "Home")
                };
            }
            catch
            {
                ViewBag.Error = "An unexpected error occurred while attempting to log in. Please try again later.";
                return View();
            }
        }

        // ------------------- LOGOUT -------------------
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}


