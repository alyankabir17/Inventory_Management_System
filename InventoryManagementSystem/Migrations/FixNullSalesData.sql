-- Fix NULL values in Sales table after migration
-- This script updates existing sales records to have proper default values

USE [InventoryDB];
GO

PRINT '=== Fixing NULL values in Sales table ===';
GO

BEGIN TRANSACTION;

-- Update any NULL CustomerName values with a default
UPDATE Sales 
SET CustomerName = 'Legacy Customer'
WHERE CustomerName IS NULL;

PRINT 'Updated NULL CustomerName values';

-- Update any NULL CustomerContact values (already nullable, but ensure consistency)
UPDATE Sales 
SET CustomerContact = NULL
WHERE CustomerContact IS NULL;

-- Ensure IsFavoriteCustomer is set correctly
UPDATE Sales 
SET IsFavoriteCustomer = 0
WHERE IsFavoriteCustomer IS NULL;

PRINT 'Updated IsFavoriteCustomer values';

-- Ensure FavoriteCustomerId is properly set
UPDATE Sales 
SET FavoriteCustomerId = NULL
WHERE FavoriteCustomerId IS NULL OR FavoriteCustomerId = 0;

PRINT 'Updated FavoriteCustomerId values';

COMMIT TRANSACTION;
GO

-- Verify the fixes
PRINT '';
PRINT '=== Verification ===';
SELECT 
    COUNT(*) as TotalSales,
    SUM(CASE WHEN CustomerName IS NULL THEN 1 ELSE 0 END) as NullCustomerNames,
    SUM(CASE WHEN IsFavoriteCustomer = 1 THEN 1 ELSE 0 END) as FavoriteCustomerSales,
    SUM(CASE WHEN IsFavoriteCustomer = 0 THEN 1 ELSE 0 END) as WalkInSales
FROM Sales;

PRINT '';
PRINT '=== Sample Data ===';
SELECT TOP 10 
    Id,
    ProductId,
    CustomerName,
    FavoriteCustomerId,
    IsFavoriteCustomer,
    Quantity,
    TotalAmount,
    SaleDate
FROM Sales
ORDER BY Id DESC;

GO
