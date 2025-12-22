# Admin-Only Access Control Implementation

## Overview
This update implements strict role-based access control where only ONE admin account exists and can manage staff members. Regular staff members have no administrative privileges.

## Changes Made

### 1. **Database Schema Changes**
- **File**: `Models/Entities/User.cs` (Staff entity)
- **Change**: Added `Role` property with default value "Staff"
  ```csharp
  public string Role { get; set; } = "Staff";
  ```

### 2. **Migration Created**
- **File**: `Migrations/20251222180000_AddRoleToStaff.cs`
- **Purpose**: Adds Role column to Staff table
- **Special Action**: Automatically sets existing admin account to "Admin" role
- **Apply with**: Stop app, then restart (automatic migration on startup)

### 3. **Admin Account Configuration**
- **File**: `Program.cs`
- **Change**: Hardcoded admin is now created with Role = "Admin"
- **Username**: `admin`
- **Password**: `admin123`
- **Role**: `Admin` (unique - only one admin exists)

### 4. **Controller Security Updates**
- **File**: `Controllers/UserController.cs`
- **Changes**:
  - Added `IsAdmin()` helper method to check role
  - All staff management actions now require Admin role
  - New staff members are ALWAYS created with Role = "Staff"
  - Admin role cannot be transferred or changed
  - Admin account cannot be edited or deleted
  - **REMOVED**: Signup functionality (only admin creates accounts)

### 5. **UI Security Updates**

#### Sidebar Navigation (`Views/Shared/_DashLayout.cshtml`)
- Staff Manager link only visible to Admin role
- Regular staff members don't see staff management options

#### Staff List (`Views/User/Index.cshtml`)
- Shows Role badge (Admin with crown icon, Staff with regular badge)
- Admin accounts show "Protected" button (no edit/delete)
- Regular staff can be edited and deleted by admin

#### Login Page (`Views/User/Login.cshtml`)
- Removed signup link
- Added message: "Only administrators can create staff accounts"
- Shows default admin credentials

#### Dashboard (`Controllers/DashboardController.cs`)
- Now passes UserRole to view for role-based UI rendering

## Security Features

### ? What's Protected:
1. **Single Admin**: Only one admin account exists (the hardcoded one)
2. **No Privilege Escalation**: Staff members cannot become admins
3. **Admin-Only Staff Management**: Only admin can create/edit/delete staff
4. **Protected Admin Account**: Admin account cannot be modified or deleted
5. **No Public Signup**: Removed signup page - only admin creates accounts
6. **Role Stored in Session**: User role tracked throughout session

### ? Access Control Matrix:

| Action | Admin | Staff | Guest |
|--------|-------|-------|-------|
| Login | ? | ? | ? |
| Signup | ? | ? | ? |
| View Staff List | ? | ? | ? |
| Create Staff | ? | ? | ? |
| Edit Staff | ? | ? | ? |
| Delete Staff | ? (except admin) | ? | ? |
| Access Dashboard | ? | ? | ? |
| Use POS System | ? | ? | ? |
| Manage Products | ? | ? | ? |

## How to Apply Changes

### Option 1: Automatic (Recommended)
1. **Stop** the running application (Shift+F5)
2. **Start** the application (F5)
3. Migration will apply automatically on startup
4. Login with admin credentials

### Option 2: Manual Database Update
If you prefer manual control:
```sql
-- Add Role column
ALTER TABLE Staff ADD Role NVARCHAR(50) NOT NULL DEFAULT 'Staff';

-- Set admin role for existing admin
UPDATE Staff SET Role = 'Admin' WHERE Username = 'admin';
```

## Testing Checklist

### As Admin:
- [ ] Login with admin/admin123
- [ ] See "Staff Manager" in sidebar
- [ ] View staff list with Role column
- [ ] Create new staff member
- [ ] Verify new staff has "Staff" role (not Admin)
- [ ] Edit regular staff member
- [ ] Cannot edit/delete admin account (shows Protected)
- [ ] Logout

### As Staff:
- [ ] Login with staff credentials
- [ ] Do NOT see "Staff Manager" in sidebar
- [ ] Cannot access /User/Index directly (should redirect to login)
- [ ] Can use POS and other features
- [ ] Cannot access staff management

### Security:
- [ ] Signup page removed/inaccessible
- [ ] Cannot create admin users through UI
- [ ] Cannot elevate staff to admin
- [ ] Admin account protected from deletion
- [ ] Direct URL access to staff pages blocked for non-admins

## Session Variables

The application now tracks these session variables:
- `UserId` - Staff member's ID
- `Username` - Staff member's username  
- `UserRole` - Either "Admin" or "Staff"

## Important Notes

1. **One Admin Rule**: The system is designed for ONE admin account. Do not manually create additional admin users in the database.

2. **No Self-Service Registration**: Staff accounts must be created by the admin through the Staff Manager interface.

3. **Role Immutability**: Once created, roles cannot be changed through the UI. This is by design for security.

4. **Admin Protection**: The admin account (username: admin) cannot be:
   - Edited (username, password, role)
   - Deleted
   - Downgraded to Staff

5. **Migration Required**: The Role column must be added to the database. This happens automatically when you restart the application.

## Troubleshooting

### "Column 'Role' does not exist"
- Stop the application
- Restart it (migration will run automatically)
- Or manually run the SQL script above

### "Staff Manager not showing"
- Clear browser cache
- Ensure you're logged in as admin
- Check session: UserRole should be "Admin"

### "Cannot create staff members"
- Ensure you're logged in as admin
- Check that Role column exists in database
- Verify session contains UserRole = "Admin"

## Future Enhancements

Consider these additional security features:
1. Password hashing (BCrypt/ASP.NET Identity)
2. Password complexity requirements
3. Account lockout after failed login attempts
4. Two-factor authentication for admin
5. Audit logging for admin actions
6. Session timeout configuration
7. Role-based permissions matrix

---

**Status**: ? Ready for Testing  
**Version**: 2.1  
**Date**: December 22, 2025  
**Security Level**: Admin-Only Staff Management
