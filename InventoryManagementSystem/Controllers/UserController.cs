using InventoryManagementSystem.Models;
using InventoryManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Login Page
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Store");
            }
            return View();
        }

        // 2. POST: Login Logic
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Username, string Password)
        {
            var staff = _context.Staff.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (staff != null)
            {
                // Login Success -> Create Session
                HttpContext.Session.SetInt32("UserId", staff.Id);
                HttpContext.Session.SetString("Username", staff.Username); 
                HttpContext.Session.SetString("UserRole", staff.Role); // Store role

                // Go to Dashboard
                return RedirectToAction("Index", "Store");
            }

            ViewBag.Error = "Invalid Username or Password";
            return View();
        }

        // 3. Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Note: Staff management functionality (Create, Edit, Delete, Index) has been removed
        // Admin cannot add or manage staff members through the application
    }
}