using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddHybridCustomerAndStaffManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update FavoriteCustomers table with purchase tracking
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPurchaseAmount",
                table: "FavoriteCustomers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalPurchaseCount",
                table: "FavoriteCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPurchaseDate",
                table: "FavoriteCustomers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistered",
                table: "FavoriteCustomers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            // Update Sales table for hybrid customer support
            migrationBuilder.AddColumn<int>(
                name: "FavoriteCustomerId",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerContact",
                table: "Sales",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFavoriteCustomer",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Create StaffMembers table
            migrationBuilder.CreateTable(
                name: "StaffMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CellNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateOfJoining = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfLeaving = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMembers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop StaffMembers table
            migrationBuilder.DropTable(name: "StaffMembers");

            // Remove columns from Sales
            migrationBuilder.DropColumn(name: "FavoriteCustomerId", table: "Sales");
            migrationBuilder.DropColumn(name: "CustomerContact", table: "Sales");
            migrationBuilder.DropColumn(name: "IsFavoriteCustomer", table: "Sales");

            // Remove columns from FavoriteCustomers
            migrationBuilder.DropColumn(name: "TotalPurchaseAmount", table: "FavoriteCustomers");
            migrationBuilder.DropColumn(name: "TotalPurchaseCount", table: "FavoriteCustomers");
            migrationBuilder.DropColumn(name: "LastPurchaseDate", table: "FavoriteCustomers");
            migrationBuilder.DropColumn(name: "DateRegistered", table: "FavoriteCustomers");
        }
    }
}
