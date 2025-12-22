using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entities
{
    public class FavoriteCustomer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        // Track total purchases for this customer
        [Range(0, double.MaxValue)]
        public decimal TotalPurchaseAmount { get; set; } = 0;

        public int TotalPurchaseCount { get; set; } = 0;

        public DateTime? LastPurchaseDate { get; set; }

        public DateTime DateRegistered { get; set; } = DateTime.Now;
    }
}