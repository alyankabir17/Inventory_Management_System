using InventoryManagementSystem.Models.AddModels;
using InventoryManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Models
{
    public class ApplicationDbContext : DbContext
    {
        // THIS CONSTRUCTOR IS MANDATORY
        // It passes the connection string settings from Program.cs to the database
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Admin login accounts (can login to system)
        public DbSet<Staff> Staff { get; set; }
        
        // Employee records managed by admin (cannot login)
        public DbSet<StaffMember> StaffMembers { get; set; }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<FavoriteCustomer> FavoriteCustomers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Sale> Sales { get; set; }
    }
}