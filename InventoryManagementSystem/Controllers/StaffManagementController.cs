using InventoryManagementSystem.Models;
using InventoryManagementSystem.Models.AddModels;
using InventoryManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers
{
    public class StaffManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaffManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Check if user is admin
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin";
        }

        // GET: StaffManagement
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "User");
            }

            var staffMembers = await _context.StaffMembers
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.DateOfJoining)
                .ToListAsync();

            ViewBag.TotalStaff = staffMembers.Count;
            ViewBag.TotalSalary = staffMembers.Sum(s => s.Salary);
            ViewBag.InactiveStaff = await _context.StaffMembers.CountAsync(s => !s.IsActive);

            return View(staffMembers);
        }

        // GET: StaffManagement/Create
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        // POST: StaffManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddStaffMember addStaffMember)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                var staffMember = new StaffMember
                {
                    FirstName = addStaffMember.FirstName,
                    LastName = addStaffMember.LastName,
                    Email = addStaffMember.Email,
                    CellNumber = addStaffMember.CellNumber,
                    Address = addStaffMember.Address,
                    Position = addStaffMember.Position,
                    Department = addStaffMember.Department,
                    Salary = addStaffMember.Salary,
                    DateOfJoining = addStaffMember.DateOfJoining,
                    Notes = addStaffMember.Notes,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                _context.StaffMembers.Add(staffMember);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Staff member {staffMember.FullName} added successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(addStaffMember);
        }

        // GET: StaffManagement/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "User");
            }

            var staffMember = await _context.StaffMembers.FindAsync(id);
            if (staffMember == null)
            {
                return NotFound();
            }

            return View(staffMember);
        }

        // POST: StaffManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StaffMember staffMember)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "User");
            }

            if (id != staffMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    staffMember.LastModifiedDate = DateTime.Now;
                    _context.Update(staffMember);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Staff member {staffMember.FullName} updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffMemberExists(staffMember.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(staffMember);
        }

        // GET: StaffManagement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "User");
            }

            var staffMember = await _context.StaffMembers.FindAsync(id);
            if (staffMember == null)
            {
                return NotFound();
            }

            return View(staffMember);
        }

        // POST: StaffManagement/Deactivate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "User");
            }

            var staffMember = await _context.StaffMembers.FindAsync(id);
            if (staffMember != null)
            {
                staffMember.IsActive = false;
                staffMember.DateOfLeaving = DateTime.Now;
                staffMember.LastModifiedDate = DateTime.Now;
                
                _context.Update(staffMember);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Staff member {staffMember.FullName} deactivated successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: StaffManagement/Reactivate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reactivate(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "User");
            }

            var staffMember = await _context.StaffMembers.FindAsync(id);
            if (staffMember != null)
            {
                staffMember.IsActive = true;
                staffMember.DateOfLeaving = null;
                staffMember.LastModifiedDate = DateTime.Now;
                
                _context.Update(staffMember);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Staff member {staffMember.FullName} reactivated successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: StaffManagement/Inactive
        public async Task<IActionResult> Inactive()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "User");
            }

            var inactiveStaff = await _context.StaffMembers
                .Where(s => !s.IsActive)
                .OrderByDescending(s => s.DateOfLeaving)
                .ToListAsync();

            return View(inactiveStaff);
        }

        // POST: StaffManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "User");
            }

            var staffMember = await _context.StaffMembers.FindAsync(id);
            if (staffMember != null)
            {
                // Only allow deletion of inactive staff members
                if (!staffMember.IsActive)
                {
                    string staffName = staffMember.FullName;
                    _context.StaffMembers.Remove(staffMember);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Staff member {staffName} permanently deleted from the system!";
                    return RedirectToAction(nameof(Inactive));
                }
                else
                {
                    TempData["ErrorMessage"] = "Cannot delete active staff members. Please deactivate them first.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Inactive));
        }

        private bool StaffMemberExists(int id)
        {
            return _context.StaffMembers.Any(e => e.Id == id);
        }
    }
}
