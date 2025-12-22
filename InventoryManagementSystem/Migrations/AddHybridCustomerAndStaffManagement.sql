-- Migration: Add Hybrid Customer System and Staff Management
-- This script adds purchase tracking to FavoriteCustomers, hybrid customer support to Sales,
-- and creates the StaffMembers table for employee management

USE [InventoryDB];
GO

PRINT '=== Starting Migration: AddHybridCustomerAndStaffManagement ===';
GO

BEGIN TRANSACTION;

-- =====================================================
-- 1. UPDATE FAVORITECUSTOMERS TABLE
-- =====================================================
PRINT 'Updating FavoriteCustomers table...';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FavoriteCustomers]') AND name = 'TotalPurchaseAmount')
BEGIN
    ALTER TABLE FavoriteCustomers 
    ADD TotalPurchaseAmount DECIMAL(18,2) NOT NULL DEFAULT 0;
    PRINT '  - Added TotalPurchaseAmount column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FavoriteCustomers]') AND name = 'TotalPurchaseCount')
BEGIN
    ALTER TABLE FavoriteCustomers 
    ADD TotalPurchaseCount INT NOT NULL DEFAULT 0;
    PRINT '  - Added TotalPurchaseCount column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FavoriteCustomers]') AND name = 'LastPurchaseDate')
BEGIN
    ALTER TABLE FavoriteCustomers 
    ADD LastPurchaseDate DATETIME2 NULL;
    PRINT '  - Added LastPurchaseDate column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FavoriteCustomers]') AND name = 'DateRegistered')
BEGIN
    ALTER TABLE FavoriteCustomers 
    ADD DateRegistered DATETIME2 NOT NULL DEFAULT GETDATE();
    PRINT '  - Added DateRegistered column';
END

-- =====================================================
-- 2. UPDATE SALES TABLE
-- =====================================================
PRINT 'Updating Sales table...';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sales]') AND name = 'FavoriteCustomerId')
BEGIN
    ALTER TABLE Sales 
    ADD FavoriteCustomerId INT NULL;
    PRINT '  - Added FavoriteCustomerId column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sales]') AND name = 'CustomerContact')
BEGIN
    ALTER TABLE Sales 
    ADD CustomerContact NVARCHAR(100) NULL;
    PRINT '  - Added CustomerContact column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sales]') AND name = 'IsFavoriteCustomer')
BEGIN
    ALTER TABLE Sales 
    ADD IsFavoriteCustomer BIT NOT NULL DEFAULT 0;
    PRINT '  - Added IsFavoriteCustomer column';
END

-- =====================================================
-- 3. CREATE STAFFMEMBERS TABLE
-- =====================================================
PRINT 'Creating StaffMembers table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StaffMembers')
BEGIN
    CREATE TABLE StaffMembers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        CellNumber NVARCHAR(20) NOT NULL,
        Address NVARCHAR(200) NULL,
        Position NVARCHAR(100) NOT NULL,
        Department NVARCHAR(100) NOT NULL,
        Salary DECIMAL(18,2) NOT NULL,
        DateOfJoining DATETIME2 NOT NULL,
        DateOfLeaving DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        Notes NVARCHAR(500) NULL,
        CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
        LastModifiedDate DATETIME2 NULL
    );
    PRINT '  - Created StaffMembers table';
END
ELSE
BEGIN
    PRINT '  - StaffMembers table already exists';
END

COMMIT TRANSACTION;
GO

PRINT '=== Migration Complete ===';
GO

-- =====================================================
-- 4. VERIFY CHANGES
-- =====================================================
PRINT '';
PRINT '=== Verification ===';

PRINT 'FavoriteCustomers columns:';
SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.max_length AS MaxLength
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID(N'[dbo].[FavoriteCustomers]')
ORDER BY c.column_id;

PRINT '';
PRINT 'Sales columns:';
SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.max_length AS MaxLength
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID(N'[dbo].[Sales]')
ORDER BY c.column_id;

PRINT '';
PRINT 'StaffMembers columns:';
SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.max_length AS MaxLength
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID(N'[dbo].[StaffMembers]')
ORDER BY c.column_id;

GO
