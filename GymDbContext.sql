-- =============================================
-- GYM MANAGEMENT SYSTEM - FULL DATABASE SCRIPT
-- Phase 1 - Complete Schema Sync (Inventory, Equipment, Invoices, Analytics)
-- Updated: 2026-03-20
-- =============================================

USE [GymManagementDb];
GO

-- =============================================
-- 1. BASE TABLES
-- =============================================

-- Roles
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles')
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    Permissions NVARCHAR(MAX), 
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- Users
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20),
    RoleId INT NOT NULL REFERENCES Roles(Id),
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    LastLoginAt DATETIME2
);
GO

-- Members
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Members')
CREATE TABLE Members (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NULL REFERENCES Users(Id),
    MemberCode NVARCHAR(20) NOT NULL UNIQUE,
    FullName NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255),
    PhoneNumber NVARCHAR(20) NOT NULL,
    DateOfBirth DATETIME2,
    Gender NVARCHAR(20),
    Address NVARCHAR(500),
    EmergencyContact NVARCHAR(255),
    EmergencyPhone NVARCHAR(20),
    Status INT NOT NULL DEFAULT 1, -- Active
    JoinedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Note NVARCHAR(MAX),
    FaceEncoding NVARCHAR(MAX),
    QRCode NVARCHAR(255),
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- =============================================
-- 2. MEMBERSHIP & BILLING
-- =============================================

-- Membership Packages
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MembershipPackages')
CREATE TABLE MembershipPackages (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    DurationInDays INT NOT NULL,
    DurationInMonths INT NOT NULL DEFAULT 1,
    Price DECIMAL(18,2) NOT NULL,
    DiscountPrice DECIMAL(18,2),
    SessionLimit INT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- Member Subscriptions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MemberSubscriptions')
CREATE TABLE MemberSubscriptions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    MemberId UNIQUEIDENTIFIER NOT NULL REFERENCES Members(Id),
    PackageId UNIQUEIDENTIFIER NOT NULL REFERENCES MembershipPackages(Id),
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    Status INT NOT NULL DEFAULT 0, -- Pending
    RemainingSessions INT NULL,
    OriginalPackageName NVARCHAR(255) NOT NULL DEFAULT '',
    OriginalPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
    DiscountApplied DECIMAL(18,2) NOT NULL DEFAULT 0,
    FinalPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- Payments
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Payments')
CREATE TABLE Payments (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    MemberSubscriptionId UNIQUEIDENTIFIER NOT NULL REFERENCES MemberSubscriptions(Id),
    Amount DECIMAL(18,2) NOT NULL,
    Method INT NOT NULL, -- Cash, BankTransfer...
    PaymentDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Status INT NOT NULL DEFAULT 1, -- Completed
    TransactionId NVARCHAR(100),
    Note NVARCHAR(MAX),
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- =============================================
-- 3. ATTENDANCE & STAFF
-- =============================================

-- Check-ins
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CheckIns')
CREATE TABLE CheckIns (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    MemberId UNIQUEIDENTIFIER NOT NULL REFERENCES Members(Id),
    SubscriptionId UNIQUEIDENTIFIER NULL REFERENCES MemberSubscriptions(Id),
    CheckInTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CheckOutTime DATETIME2 NULL,
    Note NVARCHAR(500),
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- Trainers
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Trainers')
CREATE TABLE Trainers (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NULL REFERENCES Users(Id),
    TrainerCode NVARCHAR(20) UNIQUE,
    FullName NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255),
    PhoneNumber NVARCHAR(20),
    Specialization NVARCHAR(500),
    Salary DECIMAL(18,2),
    SessionRate DECIMAL(18,2),
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- =============================================
-- 4. INVENTORY MODULE
-- =============================================

-- Products
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
CREATE TABLE Products (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    SKU NVARCHAR(50) NOT NULL UNIQUE,
    Price DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    MinStockThreshold INT NOT NULL DEFAULT 5,
    ExpirationDate DATETIME2 NULL,
    Category NVARCHAR(100),
    ImageUrl NVARCHAR(500),
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- Stock Transactions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StockTransactions')
CREATE TABLE StockTransactions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProductId UNIQUEIDENTIFIER NOT NULL REFERENCES Products(Id),
    Type INT NOT NULL, -- Import=1, Export=2, Adjustment=3
    Quantity INT NOT NULL,
    Date DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Note NVARCHAR(MAX),
    ReferenceNumber NVARCHAR(100),
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- =============================================
-- 5. EQUIPMENT MODULE
-- =============================================

-- Equipments (Assets)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Equipments')
CREATE TABLE Equipments (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(255) NOT NULL,
    SerialNumber NVARCHAR(100),
    Category NVARCHAR(100),
    Quantity INT NOT NULL DEFAULT 1,
    PurchaseDate DATETIME2 NOT NULL,
    PurchasePrice DECIMAL(18,2) NOT NULL,
    Provider NVARCHAR(255),
    Status INT NOT NULL DEFAULT 1, -- Active
    Priority INT NOT NULL DEFAULT 2, -- Medium
    Location NVARCHAR(255),
    Description NVARCHAR(MAX),
    MaintenanceIntervalDays INT NOT NULL DEFAULT 90,
    LastMaintenanceDate DATETIME2 NULL,
    NextMaintenanceDate DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- Maintenance Logs
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MaintenanceLogs')
CREATE TABLE MaintenanceLogs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EquipmentId UNIQUEIDENTIFIER NOT NULL REFERENCES Equipments(Id),
    Date DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ScheduledDate DATETIME2 NULL,
    Cost DECIMAL(18,2) NOT NULL DEFAULT 0,
    Description NVARCHAR(MAX) NOT NULL,
    Technician NVARCHAR(255),
    Status INT NOT NULL DEFAULT 3, -- Completed
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- Equipment Transactions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'EquipmentTransactions')
CREATE TABLE EquipmentTransactions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EquipmentId UNIQUEIDENTIFIER NOT NULL REFERENCES Equipments(Id),
    Type INT NOT NULL, 
    Quantity INT NOT NULL,
    Date DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FromLocation NVARCHAR(255),
    ToLocation NVARCHAR(255),
    Note NVARCHAR(MAX),
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- =============================================
-- 6. SYSTEM LOGS
-- =============================================

-- Audit Logs
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditLogs')
CREATE TABLE AuditLogs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId NVARCHAR(100) NOT NULL,
    Action NVARCHAR(50) NOT NULL,
    EntityName NVARCHAR(100) NOT NULL,
    OldValues NVARCHAR(MAX),
    NewValues NVARCHAR(MAX),
    Timestamp DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);
GO

-- =============================================
-- 7. SEED DATA (CORE SYSTEM)
-- =============================================

-- Seed Roles
IF NOT EXISTS (SELECT * FROM Roles)
BEGIN
    INSERT INTO Roles (RoleName, Description, Permissions) VALUES 
    ('Admin', 'Full system access', '["*"]'),
    ('Manager', 'Gym operations', '["members.*", "packages.*", "subscriptions.*", "payments.*", "checkins.*", "reports.*", "inventory.*", "equipment.*"]'),
    ('Trainer', 'Classes and members', '["classes.*", "members.view", "checkins.view"]'),
    ('Receptionist', 'Daily operations', '["checkins.*", "members.*", "subscriptions.*"]');
END
GO

PRINT 'Database schema updated and synchronized successfully!';
GO