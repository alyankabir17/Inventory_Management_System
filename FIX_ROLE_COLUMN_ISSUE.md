# Fix for "Invalid column name 'Role'" Exception

## Root Cause Analysis

**Exception:** `Microsoft.Data.SqlClient.SqlException: Invalid column name 'Role'`

**Root Cause:** The `Staff` entity class has a `Role` property defined in C#, but this property was added after the initial database migration. The database table `Staff` doesn't have a `Role` column, causing Entity Framework to fail when querying the database.

### Evidence:
1. **Entity Model** - `Staff.cs` includes: `public string Role { get; set; } = "Staff";`
2. **Database Schema** - The `Staff` table is missing the `Role` column
3. **Code Dependencies** - Multiple parts of the application expect the `Role` property:
   - `UserController.Login()` - Sets `UserRole` session variable from `staff.Role`
   - `UserController.Create()` - Sets `Role = "Staff"` for new staff
   - `UserController.Edit()` - Checks and preserves `Role`
   - `UserController.Delete()` - Prevents deleting admins by checking `Role`
   - `Program.cs` - Seeds admin account with `Role = "Admin"`

## Solution

A migration file `20251222180000_AddRoleToStaff.cs` has been created to add the `Role` column to the database.

### Option 1: Apply Migration via Application Startup (Recommended)

The application is configured to automatically apply migrations on startup via `context.Database.Migrate()` in `Program.cs`.

**Steps:**
1. Stop the application if it's running
2. Ensure the migration files are present in the `Migrations` folder
3. Start the application - the migration will be applied automatically
4. Verify by logging in

### Option 2: Apply Migration via SQL Script

If automatic migration doesn't work, run the SQL script manually:

**Steps:**
1. Open SQL Server Management Studio or your preferred SQL client
2. Connect to your database
3. Open `Migrations\AddRoleColumn.sql`
4. Execute the script
5. Restart the application

### Option 3: Apply Migration via EF Core CLI

If you have the EF Core tools working:

```bash
cd "E:\study material\6th semester material\VP LAB\OEL\InventoryManagementSystem\InventoryManagementSystem"
dotnet ef database update
```

## Migration Details

The migration adds:
- A `Role` column of type `nvarchar(50)` to the `Staff` table
- Default value of `"Staff"` for the column
- Updates existing admin account to have `Role = 'Admin'`

## Verification

After applying the migration:

1. **Check Database Schema:**
   ```sql
   SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
   WHERE TABLE_NAME = 'Staff' AND COLUMN_NAME = 'Role';
   ```

2. **Check Existing Data:**
   ```sql
   SELECT Id, Username, Role FROM Staff;
   ```

3. **Test Login:** Try logging in with your credentials - the exception should be resolved.

## Files Modified

- `Migrations\20251222180000_AddRoleToStaff.cs` - Migration to add Role column
- `Migrations\ApplicationDbContextModelSnapshot.cs` - Updated to include Role property
- `Migrations\AddRoleColumn.sql` - SQL script for manual migration

## Additional Notes

- The migration is idempotent - it checks if the column exists before adding it
- All new staff accounts will default to `Role = "Staff"`
- Only the seeded admin account will have `Role = "Admin"`
- The `Role` property is required (non-nullable) with a maximum length of 50 characters
