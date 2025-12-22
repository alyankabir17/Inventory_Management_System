-- Migration: Change CustomerId to CustomerName in Sales table
-- Run this script in SQL Server Management Studio or via sqlcmd

USE [InventoryDB];
GO

PRINT '=== Starting Migration: ChangeCustomerIdToCustomerName ===';
GO

BEGIN TRANSACTION;

-- Step 1: Check and add CustomerName column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sales]') AND name = 'CustomerName')
BEGIN
    ALTER TABLE Sales 
    ADD CustomerName NVARCHAR(100) NOT NULL DEFAULT 'Walk-in Customer';
    PRINT 'Added CustomerName column to Sales table';
END
ELSE
BEGIN
    PRINT 'CustomerName column already exists';
END
GO

-- Step 2: Check and drop CustomerId column
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Sales]') AND name = 'CustomerId')
BEGIN
    ALTER TABLE Sales 
    DROP COLUMN CustomerId;
    PRINT 'Dropped CustomerId column from Sales table';
END
ELSE
BEGIN
    PRINT 'CustomerId column does not exist (already removed)';
END
GO

COMMIT TRANSACTION;
GO

PRINT '=== Migration Complete ===';
GO

-- Verify the changes
SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.max_length AS MaxLength,
    c.is_nullable AS IsNullable
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID(N'[dbo].[Sales]')
ORDER BY c.column_id;
GO
