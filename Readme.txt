AgriConnectSystem

Project Overview
AgriConnectSystem is a web-based platform developed using ASP.NET Core MVC. It enables farmers to manage agricultural products and allows employees to oversee and administer these records. It features session-based role authentication to provide secure and distinct access to functionality for both Farmers and Employees.

Features
- Product Management: Farmers can create and view their own products; Employees can manage all products.
- Role-Based Access Control: Session-driven authentication defines access based on Farmer or Employee roles.
- Dynamic Product Filtering: Products can be filtered by category.
- Form Validation and Feedback: Proper model validation with inline and alert-based feedback.
- Secure Session Handling: Unauthorized access is prevented via session checks.

Prerequisites
.NET 8.0 SDK

git clone https://github.com/JedialR/Prog7311-Part-2.git

cd Prog7311-Part-2

LocalDB


File Structure
1. Controllers
ProductController.cs: Handles product listing, creation, and filtering logic for both roles.
HomeController.cs :Basic landing and dashboard routing
FarmerController.cs: Only accessible to Employees
AccountController.cs: Handles user login/logout

2. Models
Product.cs: Defines product fields like Name, Category, ProductionDate, and FarmerId.
Farmer.cs: Links products to users; includes AppUserId, FullName, etc.
AppUser.cs: Manages login credentials and roles.

3. Views
Product/Index.cshtml: Displays filtered product lists per user role.

Product/Create.cshtml: Submission form for new products.

Shared/_Layout.cshtml: Main layout view, dynamically shows logout and hides irrelevant links.

4. Data
AgriConnectContext.cs: Entity Framework Core DB context for all entities.

Usage Guidelines
For Farmers
Log in with a Farmer account.

Navigate to Add Product to submit a new product.

View your own submitted products in the list.

For Employees
Log in with an Employee account.

Access all products through the index.

Add products on behalf of any registered farmer via a dropdown.

Product Filtering
Use the Filter input to search by product category (e.g., “Vegetables”).

Notes
Ensure your session is properly configured in Startup.cs:

Code Snippets

FarmerController:

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

References:

Microsoft (n.d.) ASP.NET Core MVC documentation. Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/ (Accessed: 10 May 2025).

Stack Overflow (2010) HttpWebRequest using Basic authentication. Available at: https://stackoverflow.com/questions/4334521/httpwebrequest-using-basic-authentication (Accessed: 11 May 2025).

OpenAI. (2025). ChatGPT [Large language model]. Available at: https://chat.openai.com (Accessed: 13 May 2025).

JetBrains (2023) Basics of ASP.NET MVC. Available at: https://www.jetbrains.com/guide/dotnet/tutorials/basics/aspnet-mvc/ (Accessed: 12 May 2025).

Microsoft (2024) Introduction to C# - Interactive Tutorials. Available at: https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/ (Accessed: 12 May 2025).

Stack Overflow (2018) How to approve or reject record on user side on admin’s approval in ASP.NET MVC 5. Available at: https://stackoverflow.com/questions/53020257 (Accessed: 14 May 2025).