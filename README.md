# ?? Inventory Management System

A full-featured **Inventory Management System** built with **ASP.NET Core MVC** and **Entity Framework Core**. This system provides comprehensive inventory tracking, point-of-sale functionality, customer management, and detailed reporting capabilities.

![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)
![C# Version](https://img.shields.io/badge/C%23-12.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-active-success)

---

## ?? Features

### ?? Authentication & Authorization
- **Secure Login System** with session management
- **Role-Based Access Control** (Admin & Staff roles)
- **Admin Dashboard** with full system access
- **Staff Management** module for employee tracking

### ?? Inventory Management
- ? **Product Catalog** - Add, edit, delete, and view products
- ??? **Category System** - Organize products by categories
- ?? **Stock Level Tracking** - Real-time inventory monitoring
- ?? **Low Stock Alerts** - Automatic notifications for items below threshold
- ?? **Inventory Valuation** - Calculate total inventory worth

### ?? Point of Sale (POS)
- ?? **POS Terminal** - Quick and intuitive sales interface
- ?? **Transaction Processing** - Process customer purchases
- ?? **Customer Management** - Track favorite customers
- ?? **Purchase History** - Detailed sales records

### ?? Supply Chain Management
- ?? **Supplier Management** - Maintain supplier database
- ?? **Purchase Orders** - Track inventory restocking
- ?? **Restock History** - Monitor purchase patterns

### ?? Reports & Analytics
- ?? **Sales Reports** - Track daily, weekly, and monthly sales
- ?? **Revenue Analytics** - Monitor financial performance
- ?? **Inventory Reports** - Stock level analysis
- ?? **Customer Analytics** - Track customer purchase behavior

### ????? Staff Management (Admin Only)
- ?? **Employee Records** - Comprehensive staff database
- ?? **Attendance Tracking** - Monitor work schedules
- ?? **Department Management** - Organize by departments
- ?? **Salary Information** - Track compensation details
- ? **Active/Inactive Status** - Manage employee status

---

## ??? Technology Stack

### Backend
- **Framework:** ASP.NET Core 8.0 MVC
- **Language:** C# 12.0
- **ORM:** Entity Framework Core 8.0
- **Database:** Microsoft SQL Server
- **Authentication:** ASP.NET Core Session Management

### Frontend
- **UI Framework:** Bootstrap 5
- **Icons:** Font Awesome 6.7.2
- **JavaScript:** jQuery
- **CSS:** Custom styling with gradients and animations

### Additional Libraries
- **IMSClassLib** - Custom inventory management logic library
- **Runtime Compilation** - Hot reload for views during development

---

## ?? Prerequisites

Before running this project, ensure you have:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or higher)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (Optional)

---

## ?? Installation & Setup

### 1?? Clone the Repository

```bash
git clone https://github.com/alyankabir17/Inventory_Management_System.git
cd InventoryManagementSystem
```

### 2?? Update Database Connection String

Open `appsettings.json` in the `InventoryManagementSystem` project and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=InventoryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace `YOUR_SERVER_NAME` with your SQL Server instance name (e.g., `localhost`, `.\\SQLEXPRESS`).

### 3?? Restore Dependencies

```bash
dotnet restore
```

### 4?? Apply Database Migrations

```bash
cd InventoryManagementSystem
dotnet ef database update
```

This will create the database and all required tables automatically.

### 5?? Run the Application

```bash
dotnet run
```

Or press **F5** in Visual Studio to start debugging.

The application will be available at:
- **HTTPS:** `https://localhost:7XXX`
- **HTTP:** `http://localhost:5XXX`

(Port numbers may vary - check the console output)

---

## ?? Default Login Credentials

A default admin account is automatically created on first run:

- **Username:** `admin`
- **Password:** `admin123`
- **Role:** Admin

?? **Important:** Change the default password after first login in a production environment!

---

## ?? Project Structure

```
InventoryManagementSystem/
??? ?? InventoryManagementSystem/          # Main web application
?   ??? ?? Controllers/                    # MVC Controllers
?   ?   ??? ProductController.cs           # Product management
?   ?   ??? CategoryController.cs          # Category management
?   ?   ??? StoreController.cs             # POS terminal
?   ?   ??? StaffManagementController.cs   # Staff management
?   ?   ??? ...
?   ??? ?? Models/
?   ?   ??? ?? Entities/                   # Database entities
?   ?   ?   ??? Product.cs
?   ?   ?   ??? Category.cs
?   ?   ?   ??? Sale.cs
?   ?   ?   ??? ...
?   ?   ??? ?? AddModels/                  # DTOs for creating records
?   ?   ??? ApplicationDbContext.cs        # EF Core DbContext
?   ??? ?? Views/                          # Razor Views
?   ?   ??? ?? Product/                    # Product views
?   ?   ??? ?? Category/                   # Category views
?   ?   ??? ?? Store/                      # POS views
?   ?   ??? ?? Shared/                     # Shared layouts
?   ?   ?   ??? _Layout.cshtml
?   ?   ?   ??? _DashLayout.cshtml
?   ?   ??? ...
?   ??? ?? wwwroot/                        # Static files (CSS, JS, images)
?   ??? ?? Migrations/                     # EF Core migrations
?   ??? appsettings.json                   # Configuration
?   ??? Program.cs                         # Application entry point
??? ?? IMSClassLib/                        # Custom business logic library
?   ??? InventoryManager.cs                # Stock management logic
??? README.md                              # This file
```

---

## ?? Database Schema

### Core Tables

| Table | Description |
|-------|-------------|
| **Products** | Product catalog with name, description, price, quantity, and category |
| **Categories** | Product categorization system |
| **Sales** | Transaction records with customer and product details |
| **Purchases** | Inventory restocking records |
| **Suppliers** | Supplier contact information |
| **FavoriteCustomers** | Loyal customer database with purchase history |
| **Staff** | Admin/Staff login accounts |
| **StaffMembers** | Employee records (managed by admin) |

### Entity Relationships

- **Product** ? **Category** (Many-to-One)
- **Sale** ? **Product** (Many-to-One)
- **Sale** ? **FavoriteCustomer** (Many-to-One, Optional)
- **Purchase** ? **Product** (Many-to-One)
- **Purchase** ? **Supplier** (Many-to-One)

---

## ?? Key Functionalities

### Product Management
```csharp
// Add new product with category
public ActionResult Create(AddProduct addProduct)
{
    Product product = new Product()
    {
        Name = addProduct.Name,
        Description = addProduct.Description,
        Quantity = addProduct.Quantity,
        UnitPrice = addProduct.UnitPrice,
        CategoryId = addProduct.CategoryId
    };
    db.Products.Add(product);
    db.SaveChanges();
}
```

### Stock Level Monitoring
```csharp
// Check stock status using IMSClassLib
var inventory = new InventoryManager();
var status = inventory.CheckStockStatus(product.Quantity);
// Returns: 1 (High), 0 (Medium), -1 (Low Stock)
```

### Sales Processing
- Automatic inventory deduction
- Customer tracking (regular or favorite)
- Purchase statistics updates
- Total amount calculation

---

## ?? User Interface

### Dashboard
- **Statistics Cards:** Quick overview of products, low stock, inventory value
- **Quick Actions:** Fast navigation to common tasks
- **Modern Design:** Gradient backgrounds and smooth animations

### POS Terminal
- **Product Grid View:** Easy product selection
- **Stock Indicators:** Color-coded availability status
- **Quick Checkout:** Streamlined purchase flow

### Product Catalog
- **Table View:** Comprehensive product listing
- **Category Badges:** Visual category identification
- **Stock Status:** Real-time availability indicators
- **Quick Actions:** Edit/Delete buttons for each product

---

## ?? Security Features

- ? **Session-Based Authentication** - Secure user sessions
- ? **Anti-Forgery Tokens** - CSRF protection on forms
- ? **Role-Based Authorization** - Restrict admin features
- ? **SQL Injection Prevention** - Parameterized queries via EF Core
- ? **Connection String Security** - Stored in appsettings.json

---

## ?? Available Migrations

```bash
# View applied migrations
dotnet ef migrations list

# Add new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Rollback to specific migration
dotnet ef database update PreviousMigrationName
```

---

## ?? Testing

### Manual Testing Workflow

1. **Login** with admin credentials
2. **Add Categories** (e.g., Electronics, Furniture, Groceries)
3. **Add Products** and assign to categories
4. **Test POS** by making test sales
5. **Check Reports** to verify data accuracy
6. **Add Suppliers** and create purchase orders
7. **Manage Staff** (Admin only)

---

## ?? Troubleshooting

### Database Connection Issues
```bash
# Verify SQL Server is running
# Check connection string in appsettings.json
# Ensure Trusted_Connection=True is correct for your setup
```

### Migration Errors
```bash
# Remove last migration
dotnet ef migrations remove

# Reset database (WARNING: Deletes all data)
dotnet ef database drop
dotnet ef database update
```

### Port Already in Use
```bash
# Change ports in Properties/launchSettings.json
# Or use:
dotnet run --urls "https://localhost:5001;http://localhost:5000"
```

---

## ?? Contributing

Contributions are welcome! Please follow these steps:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to the branch (`git push origin feature/AmazingFeature`)
5. **Open** a Pull Request

---

## ?? To-Do / Future Enhancements

- [ ] Add barcode scanning support
- [ ] Implement advanced reporting with charts
- [ ] Add email notifications for low stock
- [ ] Create mobile-responsive design improvements
- [ ] Add product image upload functionality
- [ ] Implement invoice printing (PDF generation)
- [ ] Add multi-warehouse support
- [ ] Create API endpoints for third-party integrations
- [ ] Implement backup/restore functionality
- [ ] Add audit logging for all transactions

---

## ?? License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## ????? Author

**Alyan Kabir**
- GitHub: [@alyankabir17](https://github.com/alyankabir17)
- Repository: [Inventory_Management_System](https://github.com/alyankabir17/Inventory_Management_System)

---

## ?? Support

If you encounter any issues or have questions:

1. Check the [Issues](https://github.com/alyankabir17/Inventory_Management_System/issues) page
2. Create a new issue with detailed information
3. Include error messages, screenshots, and steps to reproduce

---

## ?? Acknowledgments

- Built with **ASP.NET Core** and **Entity Framework Core**
- UI components powered by **Bootstrap 5**
- Icons provided by **Font Awesome**
- Database powered by **Microsoft SQL Server**

---

## ?? Screenshots

### Dashboard
![Dashboard](docs/screenshots/dashboard.png)
*Admin dashboard with real-time statistics and quick actions*

### Product Catalog
![Product Catalog](docs/screenshots/products.png)
*Comprehensive product listing with category badges and stock status*

### POS Terminal
![POS Terminal](docs/screenshots/pos.png)
*Point of sale interface for quick transaction processing*

### Category Management
![Categories](docs/screenshots/categories.png)
*Organize products into manageable categories*

> **Note:** Add screenshots to the `docs/screenshots/` directory for better documentation.

---

## ?? Deployment

### Deploying to Azure

1. Publish the application
2. Create Azure SQL Database
3. Update connection string in Azure App Service Configuration
4. Deploy using Visual Studio or Azure CLI

### Deploying to IIS

1. Publish the application to a folder
2. Create IIS website
3. Configure application pool (.NET CLR Version: No Managed Code)
4. Set appropriate permissions
5. Configure connection string in appsettings.json

---

## ?? Version History

- **v1.2.0** - Added category system to products
- **v1.1.0** - Implemented favorite customer tracking
- **v1.0.0** - Initial release with core functionality

---

<div align="center">

**? If you find this project helpful, please consider giving it a star!**

Made with ?? using ASP.NET Core

</div>
