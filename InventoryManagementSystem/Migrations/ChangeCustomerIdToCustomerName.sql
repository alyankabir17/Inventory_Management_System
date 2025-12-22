-- Migration: Change CustomerId to CustomerName in Sales table
-- This script updates the Sales table to use CustomerName (string) instead of CustomerId (int)

BEGIN TRANSACTION;

-- Step 1: Add the new CustomerName column with a default value
ALTER TABLE Sales 
ADD CustomerName NVARCHAR(100) NOT NULL DEFAULT 'Walk-in Customer';

-- Step 2: Drop the old CustomerId column
ALTER TABLE Sales 
DROP COLUMN CustomerId;

COMMIT TRANSACTION;

-- Verify the change
SELECT TOP 10 * FROM Sales;
