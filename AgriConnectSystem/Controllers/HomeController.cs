using Microsoft.AspNetCore.Mvc;

namespace AgriConnectSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(role))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Role = role;
            return View(); // Views/Home/Index.cshtml
        }

      

        // EMPLOYEE DASHBOARD
        public IActionResult EmployeeDashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Employee")
                return Unauthorized();

            return View(); // Views/Home/EmployeeDashboard.cshtml
        }

        // FARMER DASHBOARD
        public IActionResult FarmerDashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Farmer")
                return Unauthorized();

            return View(); // Views/Home/FarmerDashboard.cshtml
        }
    }
}
