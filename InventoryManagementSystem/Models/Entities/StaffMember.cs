using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entities
{
    /// <summary>
    /// StaffMember entity represents employees managed by admin
    /// These are staff records for HR purposes - they cannot login to the system
    /// Only the Admin user (from Staff table) can login
    /// </summary>
    public class StaffMember
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20)]
        public string CellNumber { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required")]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Salary is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be positive")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Join date is required")]
        public DateTime DateOfJoining { get; set; } = DateTime.Now;

        public DateTime? DateOfLeaving { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastModifiedDate { get; set; }

        // Computed property
        public string FullName => $"{FirstName} {LastName}";

        public int YearsOfService => DateOfLeaving.HasValue 
            ? (DateOfLeaving.Value - DateOfJoining).Days / 365
            : (DateTime.Now - DateOfJoining).Days / 365;
    }
}
