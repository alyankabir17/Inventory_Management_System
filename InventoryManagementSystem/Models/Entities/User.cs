using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entities
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public String? Email { get; set; }
        public String? Username { get; set; }
        public String? Password { get; set; }
        
        // Role: "Admin" or "Staff"
        public string Role { get; set; } = "Staff";
    }
}
