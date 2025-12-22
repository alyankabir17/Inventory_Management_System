# PowerShell script to apply the database migration
# This script applies the ChangeCustomerIdToCustomerName migration to your database

Write-Host "=== Database Migration Script ===" -ForegroundColor Cyan
Write-Host "This script will update the Sales table to use CustomerName instead of CustomerId" -ForegroundColor Yellow
Write-Host ""

# Get connection string from appsettings.json
$appsettingsPath = "E:\study material\6th semester material\VP LAB\OEL\InventoryManagementSystem\InventoryManagementSystem\appsettings.json"

if (Test-Path $appsettingsPath) {
    $config = Get-Content $appsettingsPath | ConvertFrom-Json
    $connectionString = $config.ConnectionStrings.DefaultConnection
    
    Write-Host "Connection String found in appsettings.json" -ForegroundColor Green
    Write-Host "Connection: $connectionString" -ForegroundColor Gray
    Write-Host ""
} else {
    Write-Host "ERROR: appsettings.json not found!" -ForegroundColor Red
    exit 1
}

# Extract server and database from connection string
if ($connectionString -match "Server=([^;]+).*Database=([^;]+)") {
    $server = $matches[1]
    $database = $matches[2]
    
    Write-Host "Server: $server" -ForegroundColor Cyan
    Write-Host "Database: $database" -ForegroundColor Cyan
    Write-Host ""
} else {
    Write-Host "ERROR: Could not parse connection string!" -ForegroundColor Red
    exit 1
}

# SQL Migration Script
$sqlScript = @"
-- Migration: Change CustomerId to CustomerName in Sales table
USE [$database];
GO

BEGIN TRANSACTION;

-- Step 1: Add the new CustomerName column with a default value
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

-- Step 2: Drop the old CustomerId column if it exists
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

COMMIT TRANSACTION;

-- Verify the change
SELECT TOP 10 * FROM Sales;
GO
"@

Write-Host "Attempting to apply migration using sqlcmd..." -ForegroundColor Yellow
Write-Host ""

# Try using sqlcmd
try {
    $sqlScript | sqlcmd -S $server -d $database -E
    Write-Host ""
    Write-Host "=== Migration Applied Successfully! ===" -ForegroundColor Green
    Write-Host "The Sales table now uses CustomerName instead of CustomerId" -ForegroundColor Green
} catch {
    Write-Host ""
    Write-Host "ERROR: Failed to execute migration using sqlcmd" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ""
    Write-Host "=== MANUAL STEPS ===" -ForegroundColor Yellow
    Write-Host "1. Open SQL Server Management Studio (SSMS)" -ForegroundColor White
    Write-Host "2. Connect to server: $server" -ForegroundColor White
    Write-Host "3. Select database: $database" -ForegroundColor White
    Write-Host "4. Run the SQL script from: ChangeCustomerIdToCustomerName.sql" -ForegroundColor White
}

Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
