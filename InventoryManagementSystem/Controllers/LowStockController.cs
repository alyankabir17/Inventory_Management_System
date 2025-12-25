using InventoryManagementSystem.Models;
using InventoryManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers
{
    public class LowStockController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        // Low stock threshold - can be configured in appsettings.json
        private readonly int _lowStockThreshold;

        public LowStockController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            
            // Read threshold from configuration, default to 5 if not set
            _lowStockThreshold = _configuration.GetValue<int>("InventorySettings:LowStockThreshold", 5);
        }

        /// <summary>
        /// Check if user is logged in
        /// </summary>
        private bool IsUser()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            return userId != null;
        }

        // GET: LowStock
        public async Task<IActionResult> Index()
        {
            // Check authentication
            if (!IsUser())
            {
                return RedirectToAction("Login", "User");
            }

            // Fetch all products with quantity below threshold
            var lowStockProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Quantity < _lowStockThreshold)
                .OrderBy(p => p.Quantity)  // Show lowest stock first
                .ThenBy(p => p.Name)
                .ToListAsync();

            // Pass threshold to view for display
            ViewBag.LowStockThreshold = _lowStockThreshold;
            ViewBag.TotalLowStockItems = lowStockProducts.Count;
            
            // Calculate total value of low stock items
            ViewBag.TotalLowStockValue = lowStockProducts.Sum(p => p.UnitPrice * p.Quantity);

            return View(lowStockProducts);
        }

        // Optional: GET specific product details for restocking
        public async Task<IActionResult> Details(int id)
        {
            if (!IsUser())
            {
                return RedirectToAction("Login", "User");
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.LowStockThreshold = _lowStockThreshold;
            return View(product);
        }
    }
}
