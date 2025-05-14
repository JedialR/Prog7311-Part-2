using AgriConnectSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;

namespace AgriConnectSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly AgriConnectContext _context;

        public ProductController(AgriConnectContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userId = HttpContext.Session.GetString("UserId");

            if (role == "Farmer")
            {
                var farmer = _context.Farmers.FirstOrDefault(f => f.AppUserId.ToString() == userId);
                if (farmer != null)
                {
                    var myProducts = _context.Products
                        .Where(p => p.FarmerId == farmer.Id)
                        .Include(p => p.Farmer)
                        .ToList();

                    return View(myProducts);
                }

                TempData["Error"] = "Your farmer profile could not be found. Please contact support.";
                return RedirectToAction("FarmerDashboard", "Home");
            }

            var allProducts = _context.Products.Include(p => p.Farmer).ToList();
            return View(allProducts);
        }

        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role == "Employee")
            {
                ViewBag.Farmers = _context.Farmers.ToList();
                return View();
            }

            if (role == "Farmer")
            {
                return View();
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userId = HttpContext.Session.GetString("UserId");

            if (role == "Farmer")
            {
                var farmer = _context.Farmers.FirstOrDefault(f => f.AppUserId.ToString() == userId);
                if (farmer == null)
                {
                    TempData["Error"] = "Farmer profile not found. Please contact support.";
                    return RedirectToAction("FarmerDashboard", "Home");
                }

                product.FarmerId = farmer.Id;

                ModelState.Remove("FarmerId");
                ModelState.Remove("Farmer");
            }

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                TempData["Success"] = "Product added successfully.";
                return RedirectToAction("Index");
            }

            if (role == "Employee")
            {
                ViewBag.Farmers = _context.Farmers.ToList();
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            TempData["Error"] = "Product could not be added: " + string.Join("; ", errors);
            return View(product);
        }

        public IActionResult Filter(string category)
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userId = HttpContext.Session.GetString("UserId");

            var query = _context.Products
                .Include(p => p.Farmer)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category.Contains(category.Trim()));
            }

            if (role == "Farmer")
            {
                var farmer = _context.Farmers.FirstOrDefault(f => f.AppUserId.ToString() == userId);
                if (farmer != null)
                {
                    query = query.Where(p => p.FarmerId == farmer.Id);
                }
            }

            return View("Index", query.ToList());
        }
    }
}

