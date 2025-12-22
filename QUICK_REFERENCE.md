# ?? Quick Start Guide - New Features

## Hybrid Customer Sales (Choose between Favorite & Walk-in)

### When Making a Sale:

**Option 1: Favorite Customer** (Tracked)
1. Select "Favorite Customer" radio button
2. Choose customer from dropdown
3. Purchase automatically updates customer statistics

**Option 2: Walk-in Customer** (Not Tracked)
1. Select "Walk-in Customer" radio button
2. Enter customer name manually
3. Optionally add contact info

### View Customer Statistics:
- Go to **"Favorite Customers"** in sidebar
- See total purchases, spending, and last purchase date

---

## Staff Management (Admin Only)

### Access:
1. Login as Admin
2. Click **"Staff Management"** in sidebar

### Add Employee:
1. Click "Add Staff Member"
2. Fill in:
   - Personal info (name, email, phone, address)
   - Employment info (position, department, salary, join date)
   - Optional notes
3. Submit

### Manage Employees:
- **View**: See all employee details
- **Edit**: Update information
- **Deactivate**: Remove from active list (keeps record)
- **Reactivate**: Bring back inactive staff

### Important:
? Staff members are for record-keeping only - they CANNOT login
? Only Admin can access staff management
? Deactivate instead of delete (keeps history)

---

## Database Notes

**Tables:**
- `FavoriteCustomers` - Tracks purchase history now
- `Sales` - Supports both favorite and walk-in customers
- `StaffMembers` - NEW! Employee records (separate from login)
- `Staff` - Login accounts (Admin only)

**Migration:**
- Name: `AddHybridCustomerAndStaffManagement`
- Status: ? Applied
- Recorded: ? In migration history

---

## Quick Stats Dashboard

**Shows:**
- Active staff count
- Total monthly salary
- Favorite customer count
- Today's sales
- Inventory value

---

## Need Help?

**Problem**: Can't see Staff Management link  
**Solution**: Make sure you're logged in as Admin

**Problem**: Migration errors  
**Solution**: SQL script already applied, restart application

**Problem**: Customer dropdown empty  
**Solution**: Add favorite customers first in "Favorite Customers" section

---

**Ready to use! Restart app and test!** ??
