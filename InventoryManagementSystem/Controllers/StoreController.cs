using InventoryManagementSystem.Models;
using InventoryManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Buy(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var saleModel = new Sale
            {
                ProductId = product.Id,
                TotalAmount = product.UnitPrice
            };

            // Load favorite customers for dropdown
            ViewBag.FavoriteCustomers = new SelectList(_context.FavoriteCustomers, "Id", "Name");
            ViewBag.ProductName = product.Name;
            ViewBag.StockAvailable = product.Quantity;
            ViewBag.UnitPrice = product.UnitPrice;

            return View(saleModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(Sale sale)
        {
            sale.Id = 0;

            var product = await _context.Products.FindAsync(sale.ProductId);

            if (product == null) return NotFound();

            if (sale.Quantity > product.Quantity)
            {
                ModelState.AddModelError("Quantity", $"Error: Only {product.Quantity} piece(s) left in stock!");
            }

            if (ModelState.IsValid)
            {
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
                        
                        // Update favorite customer purchase statistics
                        favoriteCustomer.TotalPurchaseAmount += sale.TotalAmount;
                        favoriteCustomer.TotalPurchaseCount += 1;
                        favoriteCustomer.LastPurchaseDate = DateTime.Now;
                        
                        _context.FavoriteCustomers.Update(favoriteCustomer);
                    }
                }
                else
                {
                    sale.IsFavoriteCustomer = false;
                }

                // Update product quantity
                product.Quantity = product.Quantity - sale.Quantity;

                _context.Sales.Add(sale);
                _context.Products.Update(product);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Reload data for view in case of validation errors
            ViewBag.FavoriteCustomers = new SelectList(_context.FavoriteCustomers, "Id", "Name", sale.FavoriteCustomerId);
            ViewBag.ProductName = product.Name;
            ViewBag.StockAvailable = product.Quantity;
            ViewBag.UnitPrice = product.UnitPrice;

            return View(sale);
        }
    }
}
