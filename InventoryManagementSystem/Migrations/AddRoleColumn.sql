-- Migration: Add Role column to Staff table
-- This script adds the Role column with a default value of 'Staff'
-- and updates the admin account to have the 'Admin' role

-- Check if Role column already exists
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Staff]') 
               AND name = 'Role')
BEGIN
    -- Add Role column with default value
    ALTER TABLE [Staff]
    ADD [Role] nvarchar(50) NOT NULL DEFAULT 'Staff';
    
    -- Update existing admin account to Admin role
    UPDATE [Staff] 
    SET [Role] = 'Admin' 
    WHERE [Username] = 'admin';
    
    PRINT 'Role column added successfully to Staff table';
END
ELSE
BEGIN
    PRINT 'Role column already exists in Staff table';
END
GO
