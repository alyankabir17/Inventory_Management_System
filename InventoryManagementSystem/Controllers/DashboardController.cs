using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InventoryManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext db;

        public DashboardController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public ActionResult Index()
        {
            // Check if user is logged in
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "User");
            }

            // Get dashboard statistics
            ViewBag.Username = HttpContext.Session.GetString("Username") ?? "User";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole") ?? "Staff";
            ViewBag.ProductsCount = db.Products.Count();
            ViewBag.LowStock = db.Products.Count(p => p.Quantity < 5);
            ViewBag.TotalInventoryValue = db.Products.Sum(p => (decimal?)p.UnitPrice * p.Quantity) ?? 0;
            
            // Update to show StaffMembers count instead of Staff login accounts
            ViewBag.UsersCount = db.StaffMembers.Count(s => s.IsActive);
            ViewBag.FavoriteCustomersCount = db.FavoriteCustomers.Count();
            ViewBag.TodaySales = db.Sales.Count(s => s.SaleDate.Date == DateTime.Today);

            return View();
        }
    }
}