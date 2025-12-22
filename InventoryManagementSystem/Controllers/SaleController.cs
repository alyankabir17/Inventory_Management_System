using InventoryManagementSystem.Models;
using InventoryManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers
{
    public class SaleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SaleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sale
        public async Task<IActionResult> Index()
        {
            var sales = await _context.Sales.ToListAsync();
            return View(sales);
        }

        // GET: Sale/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            ViewBag.FavoriteCustomers = new SelectList(_context.FavoriteCustomers, "Id", "Name");
            return View();
        }

        // POST: Sale/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sale sale)
        {
            // Check Stock Level First
            var product = await _context.Products.FindAsync(sale.ProductId);

            if (product != null)
            {
                if (product.Quantity < sale.Quantity)
                {
                    ModelState.AddModelError("Quantity", "Not enough stock available!");
                }
                else if (ModelState.IsValid)
                {
                    // Set sale price and calculate total
                    sale.SoldPrice = product.UnitPrice;
                    sale.TotalAmount = sale.Quantity * sale.SoldPrice;
                    sale.SaleDate = DateTime.Now;

                    // Check if this is a favorite customer purchase
                    if (sale.FavoriteCustomerId.HasValue && sale.FavoriteCustomerId.Value > 0)
                    {
                        var favoriteCustomer = await _context.FavoriteCustomers.FindAsync(sale.FavoriteCustomerId.Value);
                        if (favoriteCustomer != null)
                        {
                            sale.IsFavoriteCustomer = true;
                            sale.CustomerName = favoriteCustomer.Name;
                            
                            // Update favorite customer statistics
                            favoriteCustomer.TotalPurchaseAmount += sale.TotalAmount;
                            favoriteCustomer.TotalPurchaseCount += 1;
                            favoriteCustomer.LastPurchaseDate = DateTime.Now;
                            
                            _context.FavoriteCustomers.Update(favoriteCustomer);
                        }
                    }

                    // 1. Create Sale Record
                    _context.Add(sale);

                    // 2. Decrease Stock
                    product.Quantity -= sale.Quantity;
                    _context.Update(product);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", sale.ProductId);
            ViewBag.FavoriteCustomers = new SelectList(_context.FavoriteCustomers, "Id", "Name", sale.FavoriteCustomerId);
            return View(sale);
        }
    }
}