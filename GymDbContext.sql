-- GYM MANAGEMENT SYSTEM - COMPREHENSIVE DATA SEED V24 (VALID LOGIN HASH)
-- Đảm bảo mật khẩu 123456 chuẩn ASP.NET Identity (Sửa lỗi Base-64)
-- =========================================================================

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

PRINT N'🧹 BẮT ĐẦU XÓA DỮ LIỆU CŨ THEO THỨ TỰ RÀNG BUỘC KHÓA NGOẠI...';
DELETE FROM [AuditLogs]; DELETE FROM [CheckIns]; DELETE FROM [ClassEnrollments]; DELETE FROM [Classes];
DELETE FROM [Payments]; DELETE FROM [InvoiceDetails]; DELETE FROM [Invoices]; DELETE FROM [OrderDetails]; DELETE FROM [Orders];
DELETE FROM [MaintenanceMaterials]; DELETE FROM [MaintenanceLogs]; DELETE FROM [IncidentLogs]; DELETE FROM [Depreciations];
DELETE FROM [EquipmentTransactions]; DELETE FROM [EquipmentProviderHistories]; DELETE FROM [Equipments]; DELETE FROM [EquipmentCategories];
DELETE FROM [StockTransactions]; DELETE FROM [Inventories]; DELETE FROM [Warehouses]; DELETE FROM [Products]; DELETE FROM [Providers];
DELETE FROM [MemberSubscriptions]; DELETE FROM [MembershipPackages]; DELETE FROM [TrainerMemberAssignments]; DELETE FROM [Trainers];
DELETE FROM [Members]; DELETE FROM [UserRoles]; DELETE FROM [Users]; DELETE FROM [Roles];
PRINT N'✅ ĐÃ XÓA SẠCH DỮ LIỆU CŨ.';

DECLARE @Now DATETIME2 = GETUTCDATE();
-- HASH CHUẨN CỦA "123456" SINH TỪ ASP.NET IDENTITY (KHÔNG BỊ LỖI BASE-64)
DECLARE @PwdHash NVARCHAR(MAX) = N'AQAAAAIAAYagAAAAEOSkqydcCmKumJnlCpz6cwP78xDNtrKZmQHjTplODhpn2ErPpaKqjWGwpRV2DnWF+g==';

-- =========================================================================
-- 1. ROLES
-- =========================================================================
SET IDENTITY_INSERT [Roles] ON;
INSERT INTO [Roles] ([Id], [RoleName], [Description], [Permissions], [CreatedAt]) VALUES
(1, N'Admin', N'Quản trị viên', N'["*"]', @Now),
(2, N'Manager', N'Quản lý chi nhánh', N'["*"]', @Now),
(3, N'Trainer', N'Huấn luyện viên', N'["class.read", "member.read"]', @Now),
(4, N'Receptionist', N'Lễ tân', N'["member.read", "checkin.manage"]', @Now),
(5, N'Member', N'Hội viên', N'["checkin.self"]', @Now);
SET IDENTITY_INSERT [Roles] OFF;

-- =========================================================================
-- 2. USERS (35 Dữ liệu - Mật khẩu 123456)
-- =========================================================================
INSERT INTO [Users] ([Id], [Username], [Email], [PasswordHash], [FullName], [PhoneNumber], [IsActive], [CreatedAt])
VALUES ('11111111-1111-1111-1111-111111111111', 'admin', 'admin@gym.com', @PwdHash, N'Quản Trị Hệ Thống', '0900000000', 1, @Now);
INSERT INTO [UserRoles] ([UserId], [RoleId], [AssignedAt]) VALUES ('11111111-1111-1111-1111-111111111111', 1, @Now);

CREATE TABLE #TempUsers (Id UNIQUEIDENTIFIER, RoleType NVARCHAR(50), FullName NVARCHAR(100), Email NVARCHAR(100), Phone NVARCHAR(20));

DECLARE @i INT = 1;
WHILE @i <= 35
BEGIN
    DECLARE @Uid UNIQUEIDENTIFIER = NEWID();
    DECLARE @RoleType NVARCHAR(50) = CASE WHEN @i <= 10 THEN 'Trainer' WHEN @i <= 30 THEN 'Member' ELSE 'Staff' END;
    DECLARE @Name NVARCHAR(100) = CHOOSE(@i % 10 + 1, N'Nguyễn Văn', N'Trần Thị', N'Lê Minh', N'Phạm Hoàng', N'Đinh Xuân', N'Vũ Hải', N'Đặng Thanh', N'Bùi Quốc', N'Hoàng Tuấn', N'Ngô Thị') + ' ' + CHOOSE(@i % 5 + 1, N'Anh', N'Bảo', N'Chi', N'Đạt', N'Hải') + CAST(@i AS NVARCHAR);
    
    INSERT INTO [Users] ([Id], [Username], [Email], [PasswordHash], [FullName], [PhoneNumber], [IsActive], [CreatedAt])
    VALUES (@Uid, 'user'+CAST(@i AS NVARCHAR), 'user'+CAST(@i AS NVARCHAR)+'@gym.com', @PwdHash, @Name, '09'+RIGHT('0000000'+CAST(@i AS NVARCHAR), 8), 1, @Now);
    
    INSERT INTO #TempUsers (Id, RoleType, FullName, Email, Phone) VALUES (@Uid, @RoleType, @Name, 'user'+CAST(@i AS NVARCHAR)+'@gym.com', '09'+RIGHT('0000000'+CAST(@i AS NVARCHAR), 8));
    
    DECLARE @RoleId INT = CASE WHEN @RoleType = 'Trainer' THEN 3 WHEN @RoleType = 'Member' THEN 5 ELSE 4 END;
    INSERT INTO [UserRoles] ([UserId], [RoleId], [AssignedAt]) VALUES (@Uid, @RoleId, @Now);
    SET @i = @i + 1;
END;

-- =========================================================================
-- 3. TRAINERS (10 Dữ liệu)
-- =========================================================================
INSERT INTO [Trainers] ([Id], [UserId], [FullName], [Email], [PhoneNumber], [Specialization], [HireDate], [IsActive], [CreatedAt], [IsDeleted])
SELECT TOP 10 NEWID(), Id, FullName, Email, Phone, CHOOSE(ABS(CHECKSUM(NEWID())) % 3 + 1, N'Bodybuilding', N'Yoga & Pilates', N'CrossFit'), DATEADD(MONTH, -ABS(CHECKSUM(NEWID())) % 24, @Now), 1, @Now, 0
FROM #TempUsers WHERE RoleType = 'Trainer';

-- =========================================================================
-- 4. MEMBERS (20 Dữ liệu)
-- =========================================================================
INSERT INTO [Members] ([Id], [UserId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [MemberCode], [Gender], [Address], [Status], [JoinedDate], [CreatedAt], [IsDeleted])
SELECT NEWID(), Id, FullName, Email, Phone, DATEADD(YEAR, -(18 + ABS(CHECKSUM(NEWID())) % 30), @Now), 'MB-240' + RIGHT('00' + CAST(ROW_NUMBER() OVER(ORDER BY Id) AS NVARCHAR), 3), CHOOSE(ABS(CHECKSUM(NEWID())) % 2 + 1, N'Nam', N'Nữ'), N'Hồ Chí Minh', 1, @Now, @Now, 0
FROM #TempUsers WHERE RoleType = 'Member';

-- =========================================================================
-- 5. MEMBERSHIP PACKAGES (15 Dữ liệu)
-- =========================================================================
SET @i = 1; WHILE @i <= 15
BEGIN
    INSERT INTO [MembershipPackages] ([Id], [Name], [Description], [DurationInDays], [DurationInMonths], [Price], [DiscountPrice], [SessionLimit], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (NEWID(), CHOOSE(@i % 5 + 1, N'Gói Starter', N'Gói Pro', N'Gói VIP', N'Gói PT 1-1', N'Gói Sinh Viên') + ' ' + CAST((@i%3+1)*3 AS NVARCHAR) + N' Tháng', N'Mô tả', (@i%3+1)*90, (@i%3+1)*3, 500000 * @i, 450000 * @i, CASE WHEN @i%4=0 THEN 12 ELSE NULL END, 1, @Now, 0);
    SET @i = @i + 1;
END;

-- =========================================================================
-- 6. SUBSCRIPTIONS & PAYMENTS
-- =========================================================================
CREATE TABLE #TempSubs (SubId UNIQUEIDENTIFIER, MemberId UNIQUEIDENTIFIER, EndDate DATETIME2);
DECLARE @MemCursor CURSOR; SET @MemCursor = CURSOR FOR SELECT Id FROM [Members]; OPEN @MemCursor;
DECLARE @MemId UNIQUEIDENTIFIER; FETCH NEXT FROM @MemCursor INTO @MemId;
WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE @PkgId UNIQUEIDENTIFIER; SELECT TOP 1 @PkgId = Id FROM [MembershipPackages] ORDER BY NEWID();
    DECLARE @SubId UNIQUEIDENTIFIER = NEWID(); DECLARE @Ends DATETIME2 = DATEADD(MONTH, 3, @Now);
    INSERT INTO [MemberSubscriptions] ([Id], [MemberId], [PackageId], [StartDate], [EndDate], [Status], [OriginalPackageName], [OriginalPrice], [FinalPrice], [DiscountApplied], [CreatedAt], [IsDeleted])
    VALUES (@SubId, @MemId, @PkgId, @Now, @Ends, 2, N'Gói tập thực tế', 1500000, 1500000, 0, @Now, 0);
    INSERT INTO #TempSubs VALUES (@SubId, @MemId, @Ends);
    INSERT INTO [Payments] ([Id], [MemberSubscriptionId], [Amount], [PaymentDate], [Method], [Status], [TransactionId], [CreatedAt], [IsDeleted])
    VALUES (NEWID(), @SubId, 1500000, @Now, ABS(CHECKSUM(NEWID())) % 3 + 1, 2, 'TXN-' + CAST(ABS(CHECKSUM(NEWID())) AS NVARCHAR), @Now, 0);
    FETCH NEXT FROM @MemCursor INTO @MemId;
END;
CLOSE @MemCursor; DEALLOCATE @MemCursor;

-- =========================================================================
-- 7. WAREHOUSES & PROVIDERS & PRODUCTS
-- =========================================================================
INSERT INTO [Warehouses] ([Id], [Name], [Description], [Location], [IsActive], [CreatedAt], [IsDeleted])
VALUES (NEWID(), N'Kho Tổng', N'Kho chính', N'Tầng hầm', 1, @Now, 0), (NEWID(), N'Kho Quầy', N'Tiện ích', N'Quầy lễ tân', 1, @Now, 0);

SET @i = 1; WHILE @i <= 10
BEGIN
    DECLARE @PvId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO [Providers] ([Id], [Name], [Code], [ContactPerson], [Email], [PhoneNumber], [Address], [TaxCode], [SupplyType], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@PvId, N'NCC Fitness ' + CAST(@i AS NVARCHAR), 'PROV-'+CAST(@i AS NVARCHAR), N'Đại diện '+CAST(@i AS NVARCHAR), 'prov'+CAST(@i AS NVARCHAR)+'@gym.com', '0901111'+RIGHT('00'+CAST(@i AS NVARCHAR), 2), N'HCM', '03'+CAST(@i AS NVARCHAR), N'Tổng hợp', 1, @Now, 0);
    
    DECLARE @WhId UNIQUEIDENTIFIER; SELECT TOP 1 @WhId = Id FROM [Warehouses] ORDER BY NEWID();
    DECLARE @PdId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO [Products] ([Id], [Name], [Description], [SKU], [Price], [CostPrice], [Unit], [Type], [StockQuantity], [TrackInventory], [Category], [IsActive], [ProviderId], [CreatedAt], [IsDeleted])
    VALUES (@PdId, N'Sản phẩm mẫu '+CAST(@i AS NVARCHAR), N'Mô tả SP', 'SKU-'+CAST(@i AS NVARCHAR), 100000, 60000, N'Hộp', 3, 100, 1, N'Phụ kiện', 1, @PvId, @Now, 0);
    INSERT INTO [Inventories] ([Id], [ProductId], [WarehouseId], [Quantity], [LastUpdated], [CreatedAt], [IsDeleted])
    VALUES (NEWID(), @PdId, @WhId, 100, @Now, @Now, 0);
    SET @i = @i + 1;
END;

-- =========================================================================
-- 8. EQUIPMENTS & MAINTENANCE & DEPRECIATION
-- =========================================================================
INSERT INTO [EquipmentCategories] ([Id], [Name], [Code], [Description], [CreatedAt], [IsDeleted])
VALUES (NEWID(), N'Máy chạy Cardio', 'CARDIO', N'Dây chuyền máy chạy', @Now, 0), (NEWID(), N'Giàn tạ Strength', 'WEIGHT', N'Khu vực đẩy tạ', @Now, 0);

SET @i = 1; WHILE @i <= 15
BEGIN
    DECLARE @EqCtId UNIQUEIDENTIFIER; SELECT TOP 1 @EqCtId = Id FROM [EquipmentCategories] ORDER BY NEWID();
    DECLARE @EqPvId UNIQUEIDENTIFIER; SELECT TOP 1 @EqPvId = Id FROM [Providers] ORDER BY NEWID();
    DECLARE @EqId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO [Equipments] ([Id], [Name], [EquipmentCode], [CategoryId], [ProviderId], [Quantity], [PurchasePrice], [PurchaseDate], [MonthlyDepreciationAmount], [RemainingValue], [Status], [Priority], [Location], [CreatedAt], [IsDeleted])
    VALUES (@EqId, N'Thiết bị Gym '+CAST(@i AS NVARCHAR), 'EQ-'+CAST(@i AS NVARCHAR), @EqCtId, @EqPvId, 1, 25000000, @Now, 500000, 15000000, 1, 2, N'Tầng '+CAST(@i%3+1 AS NVARCHAR), @Now, 0);
    
    -- Bản ghi khấu hao
    INSERT INTO [Depreciations] ([Id], [EquipmentId], [Date], [Amount], [RemainingValue], [CreatedAt], [IsDeleted])
    VALUES (NEWID(), @EqId, DATEADD(MONTH, -1, @Now), 500000, 24500000, @Now, 0);
    SET @i = @i + 1;
END;

-- =========================================================================
-- 9. CLASSES & TRAINING (GROUP & PT 1-1)
-- =========================================================================
DECLARE @TrId UNIQUEIDENTIFIER; SELECT TOP 1 @TrId = Id FROM [Trainers];
DECLARE @MbId UNIQUEIDENTIFIER; SELECT TOP 1 @MbId = Id FROM [Members];

-- 10 Lớp nhóm
SET @i = 1; WHILE @i <= 10
BEGIN
    DECLARE @ClId UNIQUEIDENTIFIER = NEWID();
    DECLARE @ClType NVARCHAR(50) = CHOOSE(@i % 4 + 1, N'Yoga', N'Zumba', N'Boxing', N'HIIT');
    DECLARE @Day NVARCHAR(20) = CHOOSE(@i % 7 + 1, N'Monday', N'Tuesday', N'Wednesday', N'Thursday', N'Friday', N'Saturday', N'Sunday');

    INSERT INTO [Classes] ([Id], [ClassName], [ClassType], [Description], [TrainerId], [ScheduleDay], [StartTime], [EndTime], [MaxCapacity], [CurrentEnrollment], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@ClId, @ClType + N' Advance ' + CAST(@i AS NVARCHAR), @ClType, N'Lớp tập nhóm chuyên sâu', @TrId, @Day, '08:00:00', '09:30:00', 20, 5, 1, @Now, 0);

    -- Đăng ký 5 học viên vào lớp nhóm này
    INSERT INTO [ClassEnrollments] ([Id], [ClassId], [MemberId], [EnrolledDate], [IsAttended], [CreatedAt], [IsDeleted])
    SELECT TOP 5 NEWID(), @ClId, Id, @Now, 0, @Now, 0 FROM [Members] ORDER BY NEWID();
    
    SET @i = @i + 1;
END;

-- 10 Buổi tập PT 1-1 (Lịch cá nhân PT)
SET @i = 1; WHILE @i <= 10
BEGIN
    DECLARE @PtClId UNIQUEIDENTIFIER = NEWID();
    DECLARE @PtDay NVARCHAR(20) = CHOOSE(@i % 7 + 1, N'Monday', N'Tuesday', N'Wednesday', N'Thursday', N'Friday', N'Saturday', N'Sunday');
    DECLARE @PtStart TIME = CHOOSE(@i % 3 + 1, '10:00:00', '15:00:00', '18:00:00');

    INSERT INTO [Classes] ([Id], [ClassName], [ClassType], [Description], [TrainerId], [ScheduleDay], [StartTime], [EndTime], [MaxCapacity], [CurrentEnrollment], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (@PtClId, N'PT 1-1 Session ' + CAST(@i AS NVARCHAR), N'PT 1-1', N'Buổi tập huấn luyện viên cá nhân', @TrId, @PtDay, @PtStart, DATEADD(HOUR, 1, CAST(@PtStart AS DATETIME)), 1, 1, 1, @Now, 0);

    -- Đăng ký 1 học viên duy nhất cho PT 1-1 (IsAttended mặc định 0)
    INSERT INTO [ClassEnrollments] ([Id], [ClassId], [MemberId], [EnrolledDate], [IsAttended], [CreatedAt], [IsDeleted])
    SELECT TOP 1 NEWID(), @PtClId, Id, @Now, 0, @Now, 0 FROM [Members] ORDER BY NEWID();

    SET @i = @i + 1;
END;

PRINT N'✅ ĐÃ NẠP TOÀN BỘ DỮ LIỆU V25 - CÓ LỚP HỌC & PT 1-1!';
DROP TABLE #TempUsers; 
DROP TABLE #TempSubs; 
GO

