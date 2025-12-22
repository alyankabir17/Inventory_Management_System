using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entities
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        // Nullable - only set if customer is a favorite/registered customer
        public int? FavoriteCustomerId { get; set; }

        // Always required - either from favorite customer or manual entry
        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters")]
        public string CustomerName { get; set; } = string.Empty;

        // Optional - contact information for manual entries
        [StringLength(100)]
        public string? CustomerContact { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        public decimal SoldPrice { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.Now;

        // Track if this was a favorite customer purchase
        public bool IsFavoriteCustomer { get; set; } = false;
    }
}