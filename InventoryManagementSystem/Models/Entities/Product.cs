using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Foreign Key for Category
        public int? CategoryId { get; set; }

        // Navigation Property
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}