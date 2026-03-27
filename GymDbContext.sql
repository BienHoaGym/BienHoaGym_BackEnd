IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE TABLE [MembershipPackages] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [DiscountPrice] decimal(18,2) NULL,
        [DurationInDays] int NOT NULL,
        [DurationInMonths] int NOT NULL,
        [SessionLimit] int NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_MembershipPackages] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE TABLE [Roles] (
        [Id] int NOT NULL IDENTITY,
        [RoleName] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Permissions] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [Username] nvarchar(450) NOT NULL,
        [Email] nvarchar(450) NOT NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [FullName] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [RoleId] int NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [LastLoginAt] datetime2 NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE TABLE [Members] (
        [Id] uniqueidentifier NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Email] nvarchar(450) NOT NULL,
        [PhoneNumber] nvarchar(450) NOT NULL,
        [DateOfBirth] datetime2 NOT NULL,
        [MemberCode] nvarchar(450) NOT NULL,
        [JoinedDate] datetime2 NOT NULL,
        [Status] int NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Members] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Members_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE TABLE [Trainers] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NULL,
        [FullName] nvarchar(max) NOT NULL,
        [Email] nvarchar(450) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [Specialization] nvarchar(max) NULL,
        [Bio] nvarchar(max) NULL,
        [ProfilePhoto] nvarchar(max) NULL,
        [HireDate] datetime2 NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Trainers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Trainers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE TABLE [MemberSubscriptions] (
        [Id] uniqueidentifier NOT NULL,
        [MemberId] uniqueidentifier NOT NULL,
        [PackageId] uniqueidentifier NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        [Status] int NOT NULL,
        [RemainingSessions] int NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_MemberSubscriptions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MemberSubscriptions_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_MemberSubscriptions_MembershipPackages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [MembershipPackages] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE TABLE [Classes] (
        [Id] uniqueidentifier NOT NULL,
        [ClassName] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [TrainerId] uniqueidentifier NOT NULL,
        [ScheduleDay] nvarchar(450) NULL,
        [StartTime] time NOT NULL,
        [EndTime] time NOT NULL,
        [MaxCapacity] int NOT NULL,
        [CurrentEnrollment] int NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Classes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Classes_Trainers_TrainerId] FOREIGN KEY ([TrainerId]) REFERENCES [Trainers] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE TABLE [CheckIns] (
        [Id] uniqueidentifier NOT NULL,
        [MemberId] uniqueidentifier NOT NULL,
        [SubscriptionId] uniqueidentifier NULL,
        [CheckInTime] datetime2 NOT NULL,
        [CheckOutTime] datetime2 NULL,
        [Method] nvarchar(max) NOT NULL,
        [IsSuccessful] bit NOT NULL,
        [Note] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_CheckIns] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CheckIns_MemberSubscriptions_SubscriptionId] FOREIGN KEY ([SubscriptionId]) REFERENCES [MemberSubscriptions] ([Id]) ON DELETE SET NULL,
        CONSTRAINT [FK_CheckIns_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE TABLE [Payments] (
        [Id] uniqueidentifier NOT NULL,
        [MemberSubscriptionId] uniqueidentifier NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [PaymentDate] datetime2 NOT NULL,
        [Method] int NOT NULL,
        [Status] int NOT NULL,
        [TransactionId] nvarchar(450) NULL,
        [Note] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Payments_MemberSubscriptions_MemberSubscriptionId] FOREIGN KEY ([MemberSubscriptionId]) REFERENCES [MemberSubscriptions] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE TABLE [ClassEnrollments] (
        [Id] uniqueidentifier NOT NULL,
        [ClassId] uniqueidentifier NOT NULL,
        [MemberId] uniqueidentifier NOT NULL,
        [EnrolledDate] datetime2 NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_ClassEnrollments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClassEnrollments_Classes_ClassId] FOREIGN KEY ([ClassId]) REFERENCES [Classes] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ClassEnrollments_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'DiscountPrice', N'DurationInDays', N'DurationInMonths', N'IsActive', N'IsDeleted', N'Name', N'Price', N'SessionLimit', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[MembershipPackages]'))
        SET IDENTITY_INSERT [MembershipPackages] ON;
    EXEC(N'INSERT INTO [MembershipPackages] ([Id], [CreatedAt], [Description], [DiscountPrice], [DurationInDays], [DurationInMonths], [IsActive], [IsDeleted], [Name], [Price], [SessionLimit], [UpdatedAt])
    VALUES (''10101010-1010-1010-1010-101010101010'', ''2024-01-01T00:00:00.0000000Z'', N''Toàn bộ dịch vụ + PT cá nhân trong 365 ngày'', 4000000.0, 365, 12, CAST(1 AS bit), CAST(0 AS bit), N''Gói VIP 1 Năm'', 4500000.0, NULL, NULL),
    (''cccccccc-cccc-cccc-cccc-cccccccccccc'', ''2024-01-01T00:00:00.0000000Z'', N''Truy cập phòng gym không giới hạn trong 30 ngày'', NULL, 30, 1, CAST(1 AS bit), CAST(0 AS bit), N''Gói Cơ Bản 1 Tháng'', 500000.0, NULL, NULL),
    (''dddddddd-dddd-dddd-dddd-dddddddddddd'', ''2024-01-01T00:00:00.0000000Z'', N''Gym + Tất cả lớp học trong 30 ngày'', 750000.0, 30, 1, CAST(1 AS bit), CAST(0 AS bit), N''Gói Premium 1 Tháng'', 800000.0, NULL, NULL),
    (''eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'', ''2024-01-01T00:00:00.0000000Z'', N''10 buổi tập, hạn sử dụng 60 ngày'', NULL, 60, 2, CAST(1 AS bit), CAST(0 AS bit), N''Gói 10 Buổi Tập'', 450000.0, 10, NULL),
    (''ffffffff-ffff-ffff-ffff-ffffffffffff'', ''2024-01-01T00:00:00.0000000Z'', N''Truy cập không giới hạn 90 ngày'', 1100000.0, 90, 3, CAST(1 AS bit), CAST(0 AS bit), N''Gói 3 Tháng'', 1200000.0, NULL, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'DiscountPrice', N'DurationInDays', N'DurationInMonths', N'IsActive', N'IsDeleted', N'Name', N'Price', N'SessionLimit', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[MembershipPackages]'))
        SET IDENTITY_INSERT [MembershipPackages] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'Permissions', N'RoleName') AND [object_id] = OBJECT_ID(N'[Roles]'))
        SET IDENTITY_INSERT [Roles] ON;
    EXEC(N'INSERT INTO [Roles] ([Id], [CreatedAt], [Description], [Permissions], [RoleName])
    VALUES (1, ''2024-01-01T00:00:00.0000000Z'', N''Full system access'', N''["*"]'', N''Admin''),
    (2, ''2024-01-01T00:00:00.0000000Z'', N''Manage gym operations'', N''["members.*", "packages.*", "subscriptions.*", "payments.*", "checkins.*", "reports.*"]'', N''Manager''),
    (3, ''2024-01-01T00:00:00.0000000Z'', N''Manage classes and view members'', N''["classes.*", "members.view", "checkins.view"]'', N''Trainer''),
    (4, ''2024-01-01T00:00:00.0000000Z'', N''Check-in and basic operations'', N''["checkins.*", "members.view", "subscriptions.view"]'', N''Receptionist'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'Permissions', N'RoleName') AND [object_id] = OBJECT_ID(N'[Roles]'))
        SET IDENTITY_INSERT [Roles] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'FullName', N'IsActive', N'LastLoginAt', N'PasswordHash', N'PhoneNumber', N'RoleId', N'UpdatedAt', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] ON;
    EXEC(N'INSERT INTO [Users] ([Id], [CreatedAt], [Email], [FullName], [IsActive], [LastLoginAt], [PasswordHash], [PhoneNumber], [RoleId], [UpdatedAt], [Username])
    VALUES (''11111111-1111-1111-1111-111111111111'', ''2024-01-01T00:00:00.0000000Z'', N''admin@gym.com'', N''System Administrator'', CAST(1 AS bit), NULL, N''AQAAAAIAAYagAAAAEHfWXyU6uWwVadi4T6ReS+A/vmwhKgzPJd8YpV+aggDy1qy1YIaR0T2RO47MGCTvmA=='', N''0901234567'', 1, NULL, N''admin''),
    (''22222222-2222-2222-2222-222222222222'', ''2024-01-01T00:00:00.0000000Z'', N''manager@gym.com'', N''Nguyễn Văn Manager'', CAST(1 AS bit), NULL, N''AQAAAAIAAYagAAAAEKKm+j5Nf5ANxzCqhDAov7WsTuAdZQyEUTZ4skYTe+D/33eoc+bEjTBq5CjufVaDQw=='', N''0902345678'', 2, NULL, N''manager''),
    (''33333333-3333-3333-3333-333333333333'', ''2024-01-01T00:00:00.0000000Z'', N''trainer1@gym.com'', N''Trần Thị Hương'', CAST(1 AS bit), NULL, N''AQAAAAIAAYagAAAAEFEZlnu4vjmcKE8n+qmWHcuS5nomk7qm3ie5PA5//9Hk1sDEl1ckdKeEwmds7LAkkg=='', N''0903456789'', 3, NULL, N''trainer1''),
    (''44444444-4444-4444-4444-444444444444'', ''2024-01-01T00:00:00.0000000Z'', N''trainer2@gym.com'', N''Lê Văn Nam'', CAST(1 AS bit), NULL, N''AQAAAAIAAYagAAAAEFvDOa/gWzRHNEMRxQfbyEkkYoH4ACf+VE2msJUwVbhzopOr/z5QtMqzCJgWcDMyqg=='', N''0904567890'', 3, NULL, N''trainer2''),
    (''55555555-5555-5555-5555-555555555555'', ''2024-01-01T00:00:00.0000000Z'', N''receptionist@gym.com'', N''Phạm Thị Lan'', CAST(1 AS bit), NULL, N''AQAAAAIAAYagAAAAEEwTNWgV1DHgqWT06Lid9xxbihZ7eSiG310YqahSYdodG/xuJr/38+YnTk6jF26Acw=='', N''0905678901'', 4, NULL, N''receptionist''),
    (''66666666-6666-6666-6666-666666666666'', ''2024-02-01T00:00:00.0000000Z'', N''nguyenvana@gmail.com'', N''Nguyễn Văn A'', CAST(1 AS bit), NULL, N''AQAAAAIAAYagAAAAEN8DAiv8ruDWJvQ67OXf5fsONBX1DHMu4+qALVxE8XhK26K+qd1qhbjQUzKzWTnGDQ=='', N''0906789012'', 4, NULL, N''member001''),
    (''77777777-7777-7777-7777-777777777777'', ''2024-02-05T00:00:00.0000000Z'', N''tranthib@gmail.com'', N''Trần Thị B'', CAST(1 AS bit), NULL, N''AQAAAAIAAYagAAAAEED9fbsv8ynj2PQ6PtzZoLNyPkINoi+TJTbhqzSN7bIAqcRsGcyrQNg3PbvmRz8hgw=='', N''0907890123'', 4, NULL, N''member002''),
    (''88888888-8888-8888-8888-888888888888'', ''2024-02-10T00:00:00.0000000Z'', N''levanc@gmail.com'', N''Lê Văn C'', CAST(1 AS bit), NULL, N''AQAAAAIAAYagAAAAEBkIow4as09pKB/NjDUj5ACl49ZjytnokL0JctDQvFb0WV0Z6qy21I5GYBYvBgtYkQ=='', N''0908901234'', 4, NULL, N''member003'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'FullName', N'IsActive', N'LastLoginAt', N'PasswordHash', N'PhoneNumber', N'RoleId', N'UpdatedAt', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'DateOfBirth', N'Email', N'FirstName', N'IsDeleted', N'JoinedDate', N'LastName', N'MemberCode', N'PhoneNumber', N'Status', N'UpdatedAt', N'UserId') AND [object_id] = OBJECT_ID(N'[Members]'))
        SET IDENTITY_INSERT [Members] ON;
    EXEC(N'INSERT INTO [Members] ([Id], [CreatedAt], [DateOfBirth], [Email], [FirstName], [IsDeleted], [JoinedDate], [LastName], [MemberCode], [PhoneNumber], [Status], [UpdatedAt], [UserId])
    VALUES (''20202020-2020-2020-2020-202020202020'', ''2024-02-01T00:00:00.0000000Z'', ''1995-05-15T00:00:00.0000000'', N''nguyenvana@gmail.com'', N''Văn A'', CAST(0 AS bit), ''2024-02-01T00:00:00.0000000Z'', N''Nguyễn'', N''GYM2024001'', N''0906789012'', 1, NULL, ''66666666-6666-6666-6666-666666666666''),
    (''30303030-3030-3030-3030-303030303030'', ''2024-02-05T00:00:00.0000000Z'', ''1992-08-20T00:00:00.0000000'', N''tranthib@gmail.com'', N''Thị B'', CAST(0 AS bit), ''2024-02-05T00:00:00.0000000Z'', N''Trần'', N''GYM2024002'', N''0907890123'', 1, NULL, ''77777777-7777-7777-7777-777777777777''),
    (''40404040-4040-4040-4040-404040404040'', ''2024-02-10T00:00:00.0000000Z'', ''1998-12-10T00:00:00.0000000'', N''levanc@gmail.com'', N''Văn C'', CAST(0 AS bit), ''2024-02-10T00:00:00.0000000Z'', N''Lê'', N''GYM2024003'', N''0908901234'', 1, NULL, ''88888888-8888-8888-8888-888888888888'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'DateOfBirth', N'Email', N'FirstName', N'IsDeleted', N'JoinedDate', N'LastName', N'MemberCode', N'PhoneNumber', N'Status', N'UpdatedAt', N'UserId') AND [object_id] = OBJECT_ID(N'[Members]'))
        SET IDENTITY_INSERT [Members] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Bio', N'CreatedAt', N'Email', N'FullName', N'HireDate', N'IsActive', N'IsDeleted', N'PhoneNumber', N'ProfilePhoto', N'Specialization', N'UpdatedAt', N'UserId') AND [object_id] = OBJECT_ID(N'[Trainers]'))
        SET IDENTITY_INSERT [Trainers] ON;
    EXEC(N'INSERT INTO [Trainers] ([Id], [Bio], [CreatedAt], [Email], [FullName], [HireDate], [IsActive], [IsDeleted], [PhoneNumber], [ProfilePhoto], [Specialization], [UpdatedAt], [UserId])
    VALUES (''aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'', N''HLV Yoga với 8 năm kinh nghiệm, chứng chỉ quốc tế RYT-500'', ''2024-01-01T00:00:00.0000000Z'', N''trainer1@gym.com'', N''Trần Thị Hương'', ''2020-03-15T00:00:00.0000000'', CAST(1 AS bit), CAST(0 AS bit), N''0903456789'', NULL, N''Yoga, Pilates, Meditation'', NULL, ''33333333-3333-3333-3333-333333333333''),
    (''bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'', N''Võ sư Boxing chuyên nghiệp, từng thi đấu SEA Games'', ''2024-01-01T00:00:00.0000000Z'', N''trainer2@gym.com'', N''Lê Văn Nam'', ''2021-06-01T00:00:00.0000000'', CAST(1 AS bit), CAST(0 AS bit), N''0904567890'', NULL, N''Boxing, Muay Thai, CrossFit'', NULL, ''44444444-4444-4444-4444-444444444444'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Bio', N'CreatedAt', N'Email', N'FullName', N'HireDate', N'IsActive', N'IsDeleted', N'PhoneNumber', N'ProfilePhoto', N'Specialization', N'UpdatedAt', N'UserId') AND [object_id] = OBJECT_ID(N'[Trainers]'))
        SET IDENTITY_INSERT [Trainers] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClassName', N'CreatedAt', N'CurrentEnrollment', N'Description', N'EndTime', N'IsActive', N'IsDeleted', N'MaxCapacity', N'ScheduleDay', N'StartTime', N'TrainerId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Classes]'))
        SET IDENTITY_INSERT [Classes] ON;
    EXEC(N'INSERT INTO [Classes] ([Id], [ClassName], [CreatedAt], [CurrentEnrollment], [Description], [EndTime], [IsActive], [IsDeleted], [MaxCapacity], [ScheduleDay], [StartTime], [TrainerId], [UpdatedAt])
    VALUES (''50505050-5050-5050-5050-505050505050'', N''Yoga Buổi Sáng'', ''2024-01-01T00:00:00.0000000Z'', 0, N''Yoga cơ bản cho người mới bắt đầu'', ''07:30:00'', CAST(1 AS bit), CAST(0 AS bit), 20, N''Monday'', ''06:00:00'', ''aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'', NULL),
    (''60606060-6060-6060-6060-606060606060'', N''Boxing Tối'', ''2024-01-01T00:00:00.0000000Z'', 0, N''Boxing căn bản và nâng cao'', ''19:30:00'', CAST(1 AS bit), CAST(0 AS bit), 15, N''Wednesday'', ''18:00:00'', ''bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'', NULL),
    (''70707070-7070-7070-7070-707070707070'', N''Pilates Chiều'', ''2024-01-01T00:00:00.0000000Z'', 0, N''Pilates cho sức khỏe và vóc dáng'', ''17:00:00'', CAST(1 AS bit), CAST(0 AS bit), 12, N''Friday'', ''16:00:00'', ''aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClassName', N'CreatedAt', N'CurrentEnrollment', N'Description', N'EndTime', N'IsActive', N'IsDeleted', N'MaxCapacity', N'ScheduleDay', N'StartTime', N'TrainerId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Classes]'))
        SET IDENTITY_INSERT [Classes] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'EndDate', N'IsDeleted', N'MemberId', N'PackageId', N'RemainingSessions', N'StartDate', N'Status', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[MemberSubscriptions]'))
        SET IDENTITY_INSERT [MemberSubscriptions] ON;
    EXEC(N'INSERT INTO [MemberSubscriptions] ([Id], [CreatedAt], [EndDate], [IsDeleted], [MemberId], [PackageId], [RemainingSessions], [StartDate], [Status], [UpdatedAt])
    VALUES (''80808080-8080-8080-8080-808080808080'', ''2024-02-01T00:00:00.0000000Z'', ''2024-03-02T00:00:00.0000000'', CAST(0 AS bit), ''20202020-2020-2020-2020-202020202020'', ''dddddddd-dddd-dddd-dddd-dddddddddddd'', NULL, ''2024-02-01T00:00:00.0000000'', 2, NULL),
    (''90909090-9090-9090-9090-909090909090'', ''2024-02-05T00:00:00.0000000Z'', ''2024-04-05T00:00:00.0000000'', CAST(0 AS bit), ''30303030-3030-3030-3030-303030303030'', ''eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'', 10, ''2024-02-05T00:00:00.0000000'', 2, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'EndDate', N'IsDeleted', N'MemberId', N'PackageId', N'RemainingSessions', N'StartDate', N'Status', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[MemberSubscriptions]'))
        SET IDENTITY_INSERT [MemberSubscriptions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'CreatedAt', N'IsDeleted', N'MemberSubscriptionId', N'Method', N'Note', N'PaymentDate', N'Status', N'TransactionId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Payments]'))
        SET IDENTITY_INSERT [Payments] ON;
    EXEC(N'INSERT INTO [Payments] ([Id], [Amount], [CreatedAt], [IsDeleted], [MemberSubscriptionId], [Method], [Note], [PaymentDate], [Status], [TransactionId], [UpdatedAt])
    VALUES (''a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1'', 750000.0, ''2024-02-01T10:30:00.0000000Z'', CAST(0 AS bit), ''80808080-8080-8080-8080-808080808080'', 3, N''Thanh toán gói Premium 1 tháng'', ''2024-02-01T10:30:00.0000000Z'', 2, N''TXN20240201001'', NULL),
    (''b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2'', 450000.0, ''2024-02-05T14:15:00.0000000Z'', CAST(0 AS bit), ''90909090-9090-9090-9090-909090909090'', 1, N''Thanh toán gói 10 buổi tập'', ''2024-02-05T14:15:00.0000000Z'', 2, NULL, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amount', N'CreatedAt', N'IsDeleted', N'MemberSubscriptionId', N'Method', N'Note', N'PaymentDate', N'Status', N'TransactionId', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Payments]'))
        SET IDENTITY_INSERT [Payments] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CheckIns_CheckInTime] ON [CheckIns] ([CheckInTime]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CheckIns_MemberId] ON [CheckIns] ([MemberId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CheckIns_MemberId_CheckInTime] ON [CheckIns] ([MemberId], [CheckInTime]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CheckIns_SubscriptionId] ON [CheckIns] ([SubscriptionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ClassEnrollments_ClassId] ON [ClassEnrollments] ([ClassId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ClassEnrollments_ClassId_MemberId] ON [ClassEnrollments] ([ClassId], [MemberId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ClassEnrollments_EnrolledDate] ON [ClassEnrollments] ([EnrolledDate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ClassEnrollments_MemberId] ON [ClassEnrollments] ([MemberId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_Classes_ScheduleDay] ON [Classes] ([ScheduleDay]) WHERE [IsActive] = 1');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_Classes_ScheduleDay_StartTime] ON [Classes] ([ScheduleDay], [StartTime]) WHERE [IsActive] = 1');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Classes_TrainerId] ON [Classes] ([TrainerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Members_Email] ON [Members] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Members_MemberCode] ON [Members] ([MemberCode]) WHERE [IsDeleted] = 0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Members_PhoneNumber] ON [Members] ([PhoneNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_Members_Status] ON [Members] ([Status]) WHERE [IsDeleted] = 0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Members_UserId] ON [Members] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_MembershipPackages_IsActive] ON [MembershipPackages] ([IsActive]) WHERE [IsDeleted] = 0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_MemberSubscriptions_EndDate] ON [MemberSubscriptions] ([EndDate]) WHERE [Status] = 2');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_MemberSubscriptions_MemberId] ON [MemberSubscriptions] ([MemberId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_MemberSubscriptions_MemberId_Status] ON [MemberSubscriptions] ([MemberId], [Status]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_MemberSubscriptions_PackageId] ON [MemberSubscriptions] ([PackageId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_MemberSubscriptions_Status] ON [MemberSubscriptions] ([Status]) WHERE [IsDeleted] = 0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_MemberSubscriptionId] ON [Payments] ([MemberSubscriptionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_PaymentDate] ON [Payments] ([PaymentDate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_Status] ON [Payments] ([Status]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_Payments_TransactionId] ON [Payments] ([TransactionId]) WHERE [TransactionId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_Trainers_Email] ON [Trainers] ([Email]) WHERE [Email] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_Trainers_IsActive] ON [Trainers] ([IsActive]) WHERE [IsDeleted] = 0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Trainers_UserId] ON [Trainers] ([UserId]) WHERE [UserId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Users_RoleId] ON [Users] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Username] ON [Users] ([Username]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213045540_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260213045540_InitialCreate', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213070459_Dashboard'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAELmi40BRuMhtMrwiPb2SA3OK5LE8W5pxEYRlySb4aks3FnDT6pR6rLNOiD6WvkUApA==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213070459_Dashboard'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEIL2saMjGHgKBKol0a2zmc9GK0IM15ahgzTRiWTi9j0m+FHwIaDwNfDS1dzcqe9yWg==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213070459_Dashboard'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEEB3kLTfyzlpcm/I7r2QNs1O763HgqKAH2TB43qK9VIB/bETroc5L3ziGNQ8FCR8FA==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213070459_Dashboard'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEACe9K5dvMkXvSmlGM0cYOkU1VNyAUkZ95f8oexvVZegfmx5ASHpSTPW1MAbxPWbVQ==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213070459_Dashboard'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFR0hIlN5AuK0ml6ZD5wgK2Ub/VWCQ4uW1P5pQvfSQ1no5Mj2unAO1GmUu8/gk1Bzw==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213070459_Dashboard'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEC1h24VJSVlpd6hsbzSmfeg/og6e3TBjIpQef5bmHc26y1GocmaKCHB9NqyCD26Zrw==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213070459_Dashboard'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKpaBNpBoI8ATRBiv5Oi3YGhs0MQy17Itu6Bw+i6Iuhvh7s/N4FuXJye74V5PvSdlg==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213070459_Dashboard'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEK1pYiD+xCDi/Ek+C6JEP7J92qyS5Gkjt6Y6GWPDA176mWiGiMdW/0sKAdqXM+jsag==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213070459_Dashboard'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260213070459_Dashboard', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'FirstName');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Members] DROP COLUMN [FirstName];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'LastName');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [Members] DROP COLUMN [LastName];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    DROP INDEX [IX_Members_MemberCode] ON [Members];
    DECLARE @var2 nvarchar(max);
    SELECT @var2 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'MemberCode');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var2 + ';');
    ALTER TABLE [Members] ALTER COLUMN [MemberCode] nvarchar(20) NOT NULL;
    EXEC(N'CREATE UNIQUE INDEX [IX_Members_MemberCode] ON [Members] ([MemberCode]) WHERE [IsDeleted] = 0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    ALTER TABLE [Members] ADD [FullName] nvarchar(100) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    EXEC(N'UPDATE [Members] SET [FullName] = N''Nguyễn Văn A''
    WHERE [Id] = ''20202020-2020-2020-2020-202020202020'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    EXEC(N'UPDATE [Members] SET [FullName] = N''Trần Thị B''
    WHERE [Id] = ''30303030-3030-3030-3030-303030303030'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    EXEC(N'UPDATE [Members] SET [FullName] = N''Lê Văn C''
    WHERE [Id] = ''40404040-4040-4040-4040-404040404040'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEEPB+kw0khm2+zmuaA93mqGST0+qTLg+gOcnlSQgwq07Buojsupi1b+4QxZAexxKUw==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAELicIUqGOBiqaeDUQH7bGd+JcrvJKhOTqv5v97no2sDcrMPmc5axoABhMYmQ6aOquA==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOvEvcLetE/TBQSG0kQtU6vLiV+EuVlLMibT34rBu2jIBrVmO+GMGUeBSSWzeDcDFQ==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEETxaJ6ZVqjKKe18TQupFAh9HKuim3bQvttTKW8MPMrnhud7po3w4SgAjBLCtdfv4Q==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMfoXtRovFk0TnyAOy0pgthbLm0Atxa0DNq9VqemORHMJHaAtlqmlMXn1c0T1JYITw==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEEam9a1GTVnSL0S7Vx9P6aj1A1Ad26Bs4QsNGUhZtBXhk/QWo2MCHgsaM9ioc7kZQw==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOr/PUW4vl2jUr2VAIVQU+xcEfeI1qKbKRMxaTU1b87hYc8xWwHmqRA1Li3FRh6pdA==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJ7PhSKu2GLrC7WeBupK+f4oUO968au14BYKWnYKfWPCLkKi3/4cAcUQys9X+mDnwA==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    CREATE INDEX [IX_Members_FullName] ON [Members] ([FullName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213080348_FixMember'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260213080348_FixMember', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213084301_RefactorMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEClqrnMBMDIroIS5hHY5GV4up557lrhItzjPTWR3ZpZWzJ5NT0e9xWwNZf1jtIs84A==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213084301_RefactorMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEIQ4j7xg1hfhTJY92JWRRpzOXSkM5QIkfi8ui52iSzDXFibAfIK/7NgIVqHBR+OkBQ==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213084301_RefactorMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEHL/LGKN4L6LsuomdoOVR2UWlR9cEECM/1U0r1IaspG6Ms00PFkRD0xSp/51FiP/Aw==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213084301_RefactorMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENpNYPFdYJabPl4RPXmLXdJlvJkl6cv3J0Tesz6YSD0fDaJTfsuFsZkaTj+PD6yWBA==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213084301_RefactorMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFSdS44jklNxxLm8n23HVfRqn8eIU66xV2RR59HqXry7Bbshx2Fv9oOVLEmZzeO32A==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213084301_RefactorMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAELNLn9B2igkqz+ouDxmqg4UWSfFcxnQ9wx6aWnuexeIfHkbdEOog2FTDFo4okhEy2Q==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213084301_RefactorMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENXe+6kAiBCYp2yk3fT+w9mUQX1H5EJp4XIQsQAi+d8pO2AK0TBukm95d8ahQ3Dd9g==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213084301_RefactorMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEF1cW6wfYAMGfqxmTqELLMc9wXuG86q7RB/nutvdJPTLZKzsXffoNGAayQtYBUvrDA==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213084301_RefactorMemberFullName'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260213084301_RefactorMemberFullName', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213091610_SyncMemberFullName'
)
BEGIN
    DECLARE @var3 nvarchar(max);
    SELECT @var3 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CheckIns]') AND [c].[name] = N'IsSuccessful');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [CheckIns] DROP CONSTRAINT ' + @var3 + ';');
    ALTER TABLE [CheckIns] DROP COLUMN [IsSuccessful];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213091610_SyncMemberFullName'
)
BEGIN
    DECLARE @var4 nvarchar(max);
    SELECT @var4 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CheckIns]') AND [c].[name] = N'Method');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [CheckIns] DROP CONSTRAINT ' + @var4 + ';');
    ALTER TABLE [CheckIns] DROP COLUMN [Method];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213091610_SyncMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEO8xR8H3kdW1/Ms+5ICvP5AecAw3pb/NqfInydOWhiVOKFjhwxEAyHu7OcR3BAIHaw==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213091610_SyncMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAELYZQys+6+krI+QG6i3r1rb2fTBDROyr1G6r7SYD6LQWNazY+aKzUoVPwZfo/rs6bQ==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213091610_SyncMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEDNLNb28l6ynkF45XkYtNbr1fh1errBuDHwuxC7wwv1fVHsWr1b2c5PZ6zmscCEbew==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213091610_SyncMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFozpoOQvoviFF9gqN2DXMKoEa+a1DCQI6ose1asu5CC0GSmok9qK7alDyfRZYpNpg==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213091610_SyncMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEHpoIDraGukJs9nlMtrhHBfXwBWB1mhyFpUj+PSXfCxiev+JgnW39jrZx/5rtXRxCA==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213091610_SyncMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAED/tNn/fm5QZJ+VuiRtQolgtDFtTJF2DCMzBGxuQawFnK3AGDo5rma7gbmW3GlOlAg==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213091610_SyncMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEL/GtPVvlgDJJsWFY3ApTxLbrXRB9dRiq5eJskbavYHeKHzRbGxLxE0wEuU8PbmzDQ==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213091610_SyncMemberFullName'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEHdmlFshwh8hm2ATzWXanfeWeyNOck/UPiWDwq3qGmIyiQUcS8fRxFdVHLH+vLnN8Q==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213091610_SyncMemberFullName'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260213091610_SyncMemberFullName', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    ALTER TABLE [Members] ADD [Address] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    ALTER TABLE [Members] ADD [EmergencyContact] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    ALTER TABLE [Members] ADD [EmergencyPhone] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    ALTER TABLE [Members] ADD [Gender] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    ALTER TABLE [Members] ADD [Note] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    EXEC(N'UPDATE [Members] SET [Address] = NULL, [EmergencyContact] = NULL, [EmergencyPhone] = NULL, [Gender] = NULL, [Note] = NULL
    WHERE [Id] = ''20202020-2020-2020-2020-202020202020'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    EXEC(N'UPDATE [Members] SET [Address] = NULL, [EmergencyContact] = NULL, [EmergencyPhone] = NULL, [Gender] = NULL, [Note] = NULL
    WHERE [Id] = ''30303030-3030-3030-3030-303030303030'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    EXEC(N'UPDATE [Members] SET [Address] = NULL, [EmergencyContact] = NULL, [EmergencyPhone] = NULL, [Gender] = NULL, [Note] = NULL
    WHERE [Id] = ''40404040-4040-4040-4040-404040404040'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJm7LTIuTQub2VXLFNdtHJJihD3veTvwDnbTTIPbRZjrncPh4ouO5QCztALsSW+Oyw==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEHmEYV9prAwMIoxCNDRxcZfKaKudjbd1SSM+stIzPFi1vidxWAgGUNDiMgJUErPahQ==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAECx3Id27Kbhu6HGYtiboKpvKgOnRHt/9EHMS68i3v087TSi0HayRCsreZciwmX1/+w==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJ3ozhXQC223BTfZQ2pOP4nFO7aRorFvoob0tx1/n9FB+0H9txeA+8Z3EsUeVmTxnA==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOGpFvUTf5q+kziDKJdKK+DZpeae5+QKDZdMCnYE0jDV2frnioLKb1zMfYn4YJsdHw==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENpgA01P/ngMPjn43mJhYCdPgrBA3tW4Cr0p5jq6Vldy9Ue6UWoR08m6JeKcV8MIKw==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFUmbY7DFfa6rtjhecz+ACYUYmE/Xlbn88hNz3d9re0/OAkUUr/BG9Sgiqh9vM8mTQ==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOME/9hJ3tG4mW2937bQytVErggV+rZkuSHgQ8vtxxbyjchWOoulvj6wMXjpJyzqtw==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213112937_member'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260213112937_member', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213114807_memberdto'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEL2mJLpVyZvDqyFPdhIf0u1v6JPmOAInUz8EdBOWNifnzo0Lll6ZIAzPac3kleHepw==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213114807_memberdto'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEDrkBy3SKIM8h98Nd1ZWGstt0GbFqqmVMUZ2kt3aFX7INttHPxoq4733twhcWF4Rpg==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213114807_memberdto'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEDRnUaWx+5a9/OwL6rSKPsBQWhmjEjpGiQAZAMd+WVKe48jSscwmu0sG/pZdqFACjA==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213114807_memberdto'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEPRxMPcRlvVBikSwUmI+wR9fmyJHHTqmAZBHZYvIHcS0CtFAv9mCDlUGRSvtm9cPAQ==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213114807_memberdto'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEH7/G3OUTgZ9tihAfqt9fPYmnHvD4w5vh8E4Arh6flshYFzHahhtXPpeSNMfcYMADA==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213114807_memberdto'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENeK8qJiuyP5pUCeLbK35PNUn8NzBj7DU+87rPp7BcfcpSCc2qEbe1JoOaB2lq57Jw==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213114807_memberdto'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAED6hBr9jHduo1s7d51s+hmpY48WDpzNqQ7gztuS7EGHy1+tnYzsrzn9BiiYYMptyGQ==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213114807_memberdto'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFe/tyyxmFCVfm0yLKU0BeHtqHEXr5E+D7P4mePWgrAzQF5rBzXL11rLUQHZzfy8HQ==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213114807_memberdto'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260213114807_memberdto', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213151841_UpdateMemberNullableUser'
)
BEGIN
    DROP INDEX [IX_Members_UserId] ON [Members];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213151841_UpdateMemberNullableUser'
)
BEGIN
    DECLARE @var5 nvarchar(max);
    SELECT @var5 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'UserId');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var5 + ';');
    ALTER TABLE [Members] ALTER COLUMN [UserId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213151841_UpdateMemberNullableUser'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOsQ7h6lKcpUIhjoBSoj71Q700WAlwWUlh3rWlCxs3yST+ah2FtN9W/18oR5WBkvZA==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213151841_UpdateMemberNullableUser'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEGbs6ae9gvjxjBmqFCarwnEZOU9lmuGffC5WiaLrPxnXxKM1NG0ClgLFlyOMb6oCuA==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213151841_UpdateMemberNullableUser'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEI1+k+tDg0XAWxHckkpDDmr8lHZTLJFxN4qxKaPhwEz0oMd8qtGvXkqZiFtKxvR4fQ==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213151841_UpdateMemberNullableUser'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEBlmdkk6vM308KIeMYMEO1BoHBSOdtZPyR9osX5zg+MJBZWDlttwlsyU9LXCisMwtg==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213151841_UpdateMemberNullableUser'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFqUFKuGPsmF5jL7Q8ojqR6f6+30aJCW8hSpD7mMLsXZ1wLPpeHudQQUDcl3xaWZ0w==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213151841_UpdateMemberNullableUser'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEN9rnOOnqmXNh9J49FpQJreQplYshbo7GVyPxnT+Irt0vFF095dTo0SLSZuiz+LWqw==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213151841_UpdateMemberNullableUser'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMwr9RGDFuqMQ/zffuvY3WXnPOEzPLvyta5b+3zj0Iu3VCVbWNRQb44iaguOGzVv/w==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213151841_UpdateMemberNullableUser'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEA6SHbvqdlXWXLxPzoNYZR/1Z87+P1gI3/GXV1B3NoWN+9JlL9mUB6tb8MernQJgWA==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213151841_UpdateMemberNullableUser'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Members_UserId] ON [Members] ([UserId]) WHERE [UserId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260213151841_UpdateMemberNullableUser'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260213151841_UpdateMemberNullableUser', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    ALTER TABLE [MemberSubscriptions] ADD [DiscountApplied] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    ALTER TABLE [MemberSubscriptions] ADD [FinalPrice] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    ALTER TABLE [MemberSubscriptions] ADD [OriginalPackageName] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    ALTER TABLE [MemberSubscriptions] ADD [OriginalPrice] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    EXEC(N'UPDATE [MemberSubscriptions] SET [DiscountApplied] = 0.0, [FinalPrice] = 0.0, [OriginalPackageName] = N'''', [OriginalPrice] = 0.0
    WHERE [Id] = ''80808080-8080-8080-8080-808080808080'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    EXEC(N'UPDATE [MemberSubscriptions] SET [DiscountApplied] = 0.0, [FinalPrice] = 0.0, [OriginalPackageName] = N'''', [OriginalPrice] = 0.0
    WHERE [Id] = ''90909090-9090-9090-9090-909090909090'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAECj+HHSfWe/jGQbx9XWxkt9Ac1/u2QjiL/txW2PWPsi5Tc6BpKk9OHMIVq0CGzcOYA==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEM1Um85psFCysR1Su5n3JU1Pw/9c34TzITOoaGG4SUdtbHoJeW36Ok2mYnpl8rHa/Q==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMUNTAYneit88U63q+pQQC3QxCPVOCEhTIPMlOdhs7Z4l5lPjxBqIdxV8vi01FVa7w==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEEgG+L8iV4AumXa4dQSUBqfFFw/j2YONm/xioP5zoGknobG6Jut5qp2NCUcOgiIMaA==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEEu6jsb2yJlubiI5S0qA3NwE38TRXH7kTtq5C/O1tl7CU0Wi5pJ2DahE7KM0i75dig==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEGbTqBWqqZGUY9FI2Qm+en0DvcP0arjuGBplbCuhklkJj1marBEagFEvUVHjUi10ng==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEN0L6sZ7VN2/mAmCSEcmr5Gym0Juj7zc8U/omo8XNhIbi8O6TLrre7szu6IuAC1BBg==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEAhcJVDAQcTk0U5wxiY8RXKahdheFXTeK0XdmsMhlVBqVmh3of4bfSihNUIHQ0ACYQ==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225044452_AddSnapshotDataToSubscription'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260225044452_AddSnapshotDataToSubscription', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225050956_AddAuditLogTable'
)
BEGIN
    CREATE TABLE [AuditLogs] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        [Action] nvarchar(max) NOT NULL,
        [EntityName] nvarchar(max) NOT NULL,
        [OldValue] nvarchar(max) NOT NULL,
        [NewValue] nvarchar(max) NOT NULL,
        [Timestamp] datetime2 NOT NULL,
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225050956_AddAuditLogTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEDq3Zs5DhUYqr//ri7yN9oRe81duK2bv4eQTjlXUwriVn1lLewQO5cvzkR8qjPrzWQ==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225050956_AddAuditLogTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFejpPhGJMaTf7QUI1t23ZETsTBePJ8Cs09dd6c4zIzK/IM9odXzcTJOnPZEJrLhoA==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225050956_AddAuditLogTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEHrzR8Gz/P4KC5lwtwuFcGDXVxV5LDJC+90rQDGXwpAkGD3yJfy5p9SiEICsN2faOQ==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225050956_AddAuditLogTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJrToWRDxxQrijgnkKFInx3YnHc/xQCOn9TJgDZGos0Ss9RExVMAXSIkJ9e1jDVZeA==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225050956_AddAuditLogTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEE0d5dLPoEM719xRaj86K3i4hJB48n+6FjjN9JrarTrbzOWLrmV+9Vmjv2/YphV6Sg==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225050956_AddAuditLogTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEG2+jfMJRJrzcpHl3WMsDq8tVtjiYcvNTHP3bl2hZ5ntkkqfUFAOS5nkFscfvOPh4Q==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225050956_AddAuditLogTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEN1Sz7K6Tgvw8bJcUk2GEzgm8EFDEuFya9DB7rlH6swSFOgV9jH8Tf6jUBhAbmSVhQ==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225050956_AddAuditLogTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEEEYcmLFedZgr2KhgSGepA+4YKGb1y774M/HwOnVI3VxhBz1m8RdAXAVZC1NrPRheg==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260225050956_AddAuditLogTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260225050956_AddAuditLogTable', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    EXEC sp_rename N'[AuditLogs].[OldValue]', N'OldValues', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    EXEC sp_rename N'[AuditLogs].[NewValue]', N'NewValues', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    ALTER TABLE [AuditLogs] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    ALTER TABLE [AuditLogs] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    ALTER TABLE [AuditLogs] ADD [UpdatedAt] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEN7PvDguZSdhMsvGIef9gs9HnDIOFfCIFFtDLAn/yr41aW9HTrKygO68XXiTEz+E/w==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOl4F+bwXerCsDqS7o3OiBtrvXtWPgJSy3KjDwaA/YKWxkLaLqUit17wFW/ousyPFQ==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKgbSQqwpUoQUY7988IxdjIh0xL3yuLTWoxhgSozG/tJwaqNLAag81xeC0rjMdZISQ==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFxwFlXohf3I6KWOlto2zdTCBx9vdcnpub0r13+jiC0VGU1ueNwxfX3kINMwFXv3hw==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJbrNnkXe+bL2M6Lv/DF6q+75RgNI5jJotOHLifarZ4M4nssJ1IqvNBqlIhvcMRoMA==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJy/iXk5U6a7CZNNUyseIP2jJbYYH2gFMBnGYxVlLyP0QXc4enfFqaiVpfact3fG7w==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAECMllJrIVSR46dmRKcx7Gx7oZfIHZP3Iw1o1kfjvnDWjUZk6UWE/kiP5d/1wI14dQA==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENQwwh313O0qBvodV9w0Qg6N0bMp7huYZgzlZI5irSSFIWoCSgCSGsP0B10KZuKjZg==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260301070130_Application'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260301070130_Application', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316063157_AddExperienceYearsToTrainer'
)
BEGIN
    ALTER TABLE [Trainers] ADD [ExperienceYears] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316063157_AddExperienceYearsToTrainer'
)
BEGIN
    EXEC(N'UPDATE [Trainers] SET [ExperienceYears] = 0
    WHERE [Id] = ''aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316063157_AddExperienceYearsToTrainer'
)
BEGIN
    EXEC(N'UPDATE [Trainers] SET [ExperienceYears] = 0
    WHERE [Id] = ''bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316063157_AddExperienceYearsToTrainer'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEG255+CppNyPNcsa71Bgaofynky8hpJCXi37U8LedPEXGmAeh1S4vHHN69PVeDjUCQ==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316063157_AddExperienceYearsToTrainer'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMH1W2Rt74WwXymVhKejFNH9MICQ6SfojhGaX+D0ESScaL224Zdf1YxDZLlb9SyK5Q==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316063157_AddExperienceYearsToTrainer'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEIjr19wSVTWkxBL7e18fo5ZAI7Cnm96AvenPXZ4DPrtM04e3QzowMDFi5t62EWo3Jw==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316063157_AddExperienceYearsToTrainer'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEPYgJ6cDAqpZjL7smJk1B/+4mNEN5oNWOkmlU4nAmx3y6YKdZJFZToSaLxFUA6Iuwg==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316063157_AddExperienceYearsToTrainer'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEGVTDwukShulrwuUriWIwzv83ZyopFBzpBiHFTNPbqZHhVgB8J2Lj5JMaM7lNS230g==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316063157_AddExperienceYearsToTrainer'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEIAbh+J0yqHiJfcSqWC24/QYyI1P2wtF1CmSAh0TwYgAFX4UFukco+2+5u/o6LvDKg==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316063157_AddExperienceYearsToTrainer'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEBAoC3XAexwrOuTaiXo7h73esd6RBTuxfsBMs+5k/kEt9PJM/gTFIkkj+uENPIO+ew==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316063157_AddExperienceYearsToTrainer'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFcJqcoNkeo/FCaAdgAlJ75sGpV5rgKhQ14q1JxGuU3yHhz8f17114HyQrpi5scPDw==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316063157_AddExperienceYearsToTrainer'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260316063157_AddExperienceYearsToTrainer', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    ALTER TABLE [Members] ADD [FaceEncoding] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    ALTER TABLE [Members] ADD [QRCode] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    EXEC(N'UPDATE [Members] SET [FaceEncoding] = NULL, [QRCode] = NULL
    WHERE [Id] = ''20202020-2020-2020-2020-202020202020'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    EXEC(N'UPDATE [Members] SET [FaceEncoding] = NULL, [QRCode] = NULL
    WHERE [Id] = ''30303030-3030-3030-3030-303030303030'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    EXEC(N'UPDATE [Members] SET [FaceEncoding] = NULL, [QRCode] = NULL
    WHERE [Id] = ''40404040-4040-4040-4040-404040404040'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEGdFRXUXw7sGpQQbxXqeqXmh3puYzvHHoUfh/hDEAseeLEksyzIq/DNQPHC9rFnnkw==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEObcTew1pSkVfJw5uoCz6zChmMWWkxp51Mqu2x5s0KpATXIAa2Cyv0+twvI9+01Rxw==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEHFBRuzSw8BhoOKoodkM4VyHW22teKIMW9sgaScSyx1mP27CjzTTciGMG2nxbWdEkQ==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEGNJuUlaEyhpzXWM7SVnbqL7JpsKAxDcX9og1SayKJ8fNKLgfeNuM80Nm7ndby6gSw==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEARhx1EieDnVtW2Ug4npQza0y+5NdVr1ICRobpTjnj+jUnjLwm4tEr+R6OqPSCFk7g==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEHIuOwGsMgcdgxJhuRK5LFkHh1dqHTBEVqU+hS2KiYknkL2dGfSVlhGZ7CTaY1EMMw==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEDDTKLQYusgACAduUi/glR8SGEMa9qffplOUWCdRfKZjkY4X9CqyX/zTytf5+z6QOA==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEILdk1ilHKIMR2ls44670Lzy7s61Op/CO+J0gbkyRRV699DtbIgFx85ppXl5G68KMQ==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317053729_AddQRCodeAndFaceEncoding'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317053729_AddQRCodeAndFaceEncoding', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317055032_AddMemberQRAndFace'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEC4k3cBBY2VC0mQ9fQTlcvYV4aD8qX+jdAlKP/iUHVaa6EE2yOktQMSoTCPk0n75ng==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317055032_AddMemberQRAndFace'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEEWEStJAdF7uADKC0+rC2nge51sSbk+pek2+xCqvgzGQaBWxQFuAmKtlmOvc3zIIzw==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317055032_AddMemberQRAndFace'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJpBmqtoIlqpRcnlpQq3lk90tizOa+hEVj3ILENKF2+q6OaUsIQ/QmWC4BXStp76QQ==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317055032_AddMemberQRAndFace'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEDtCoN8pXgvooZmY8jWlMrlO5I8exB5S+YgT5hZlhWricYm16TAwOITouwJhXDO/9A==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317055032_AddMemberQRAndFace'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFFGNGfl8J6DXewbiWrzqqV5eXvqKnFQyVyEsb0Z0UKeo9fRVEFN+tXUI3urPoZj0g==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317055032_AddMemberQRAndFace'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEBrdEh7vVQFao7SHkCxH84T/ZYZLGF/usYTcUK+l2JOxkQRU8C5wmh+H5YaOcGADaw==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317055032_AddMemberQRAndFace'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEE/+QZO2oizJ0zwA/OkgMZyaN5e0Qkkuv/zzvm28lAYZrC8gE2DHE8u2RNYsgHJ0ew==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317055032_AddMemberQRAndFace'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEK5qBbSZWI7an+GJxmZyHa9TQZfoNu91MbeVLFDzQVr+AG8a5sP5mwVsQVwt/wDzMw==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317055032_AddMemberQRAndFace'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317055032_AddMemberQRAndFace', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    ALTER TABLE [Trainers] ADD [Salary] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    ALTER TABLE [Trainers] ADD [SessionRate] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    CREATE TABLE [TrainerMemberAssignments] (
        [Id] uniqueidentifier NOT NULL,
        [TrainerId] uniqueidentifier NOT NULL,
        [MemberId] uniqueidentifier NOT NULL,
        [AssignedDate] datetime2 NOT NULL,
        [Notes] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_TrainerMemberAssignments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TrainerMemberAssignments_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TrainerMemberAssignments_Trainers_TrainerId] FOREIGN KEY ([TrainerId]) REFERENCES [Trainers] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    EXEC(N'UPDATE [Trainers] SET [Salary] = 0.0, [SessionRate] = 0.0
    WHERE [Id] = ''aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    EXEC(N'UPDATE [Trainers] SET [Salary] = 0.0, [SessionRate] = 0.0
    WHERE [Id] = ''bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEAHiowP1XuD+MuTt4+8yw6LVFzqo61nog6WqXtKkSKZtTn5OGlaS6Trl1h3sBAR6hw==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENk829DXRC5n6qOhY0fDOF1ah/a8jy+KQEIx7AwzqE9ifZasqdyhRH+wHoLt+KUGZw==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEM/wSlohaIJLmXsxVk+SgfpLV1xwAb0E2v1YX0+D8kJRJGrLJaS/1xBqVW2LgtMacA==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEO1vFUy1WdeIelJpg1C+WZRU+agOB6EdflSWygUZIClknXvzvc4mtiVclQMWY/x/og==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEO+CMY4KuMxxOrxpnHcOWvr3OjCMsHiHr7gR73Iq/AdNZAv87knApWnb7lWHdHuyhA==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEDkFZU6S7EDLolWRXy974rxQJRw22JZhFGFV1D4sUm4d7YwYa0Mdn7+7cWWzK/X7qA==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOUS2s1YXve1sWiuQVwKp439fOAgVrep2cslLEpg8/dRRzFtObxmG7hdLP0OhVkzOA==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEF5WvSSMl0Uy4y7OR6fbJeBjnQs+K0XU7ahD9zsnDGdwoXXjNm+hHhCR+kQo6wnWhg==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    CREATE INDEX [IX_TrainerMemberAssignments_MemberId] ON [TrainerMemberAssignments] ([MemberId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    CREATE INDEX [IX_TrainerMemberAssignments_TrainerId] ON [TrainerMemberAssignments] ([TrainerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317061926_AddTrainerSalaryAndAssignments'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317061926_AddTrainerSalaryAndAssignments', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317070557_AddTrainerCode'
)
BEGIN
    ALTER TABLE [Trainers] ADD [TrainerCode] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317070557_AddTrainerCode'
)
BEGIN
    EXEC(N'UPDATE [Trainers] SET [TrainerCode] = N''''
    WHERE [Id] = ''aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317070557_AddTrainerCode'
)
BEGIN
    EXEC(N'UPDATE [Trainers] SET [TrainerCode] = N''''
    WHERE [Id] = ''bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317070557_AddTrainerCode'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAED5OnqhCpYpXOSXya9em8zEAWishPGIUOVcswG8w1Fx9Cco20bfanhP/b8GnyPCMxg==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317070557_AddTrainerCode'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEAQ1caH7DXv8WRyvV+Wca0NzgesXEas+xBbGEoIC7kV6Ct69Lp9KrcuCjzUJ3az5Iw==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317070557_AddTrainerCode'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEGpfpGc2r4NE32u/uI3oVUKVubuLGDqL3T9qGQWeGyoU3+RNFSLNqjJOIsetz3MEbA==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317070557_AddTrainerCode'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEBYfs31xQyl7GeS8ZYON2SNV0Q+VkD73QlKhbGzOknSwE6YbsNiAeVACiXho4ljE6A==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317070557_AddTrainerCode'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEIYV86c4zbtfK5BINqV3yVKVPx1tupUN9STK27Z+Id+nhQhuhDt2oOP9i1NAlPdngw==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317070557_AddTrainerCode'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAECkDjJ6yEakQW4cGlzCodEBpD+7/66uLzjg7YK8IoB3EKamG2AmvCTkxtTgURrHzlQ==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317070557_AddTrainerCode'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEH5AlYFA0pF9/ONmqE/EPZ3RUxyXaLXxz84pzg8wA8EWnHtMHM1bbDTOaMIdzhdP1g==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317070557_AddTrainerCode'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKPl7aH5qaKs0ly5gpbmRnwQDT7IGwYmHSc3ixJatd739QEiaGry9uJLl797YUzzUw==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317070557_AddTrainerCode'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317070557_AddTrainerCode', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    ALTER TABLE [Classes] ADD [ClassType] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    ALTER TABLE [ClassEnrollments] ADD [AttendanceDate] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    ALTER TABLE [ClassEnrollments] ADD [IsAttended] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    EXEC(N'UPDATE [Classes] SET [ClassType] = NULL
    WHERE [Id] = ''50505050-5050-5050-5050-505050505050'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    EXEC(N'UPDATE [Classes] SET [ClassType] = NULL
    WHERE [Id] = ''60606060-6060-6060-6060-606060606060'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    EXEC(N'UPDATE [Classes] SET [ClassType] = NULL
    WHERE [Id] = ''70707070-7070-7070-7070-707070707070'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJ+q+FSgBFuKw6YQaHb1HrEfy2eVs4ZtAaQpKdg+w6gkt2LyPGjH3+QLQuRbokUyrQ==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAELiazw7K1MM5HUPkzDGq2A735Z2vtNrDg6+wh3ghTb6udcLIPAzENBl6ZiaPeeU9WA==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOXVP1swWg9NUlB5A1xSPM2dUOssE7nzV1qNZWOxkFmvmEQ2jbuPhyTCira97dIZAw==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOHvYnwOKxu9N7ca9aSapSuQie0Xh1+5An/vOCjWwebO1FqWdFFm8eaka5d0OgdEnw==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEME/YBnJmp/guFuGrWon80GLpSx71jVE1O+VfiJVNiKI5bgpwymSwVo1bHGwtBVDRQ==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENBhgRjhJhD3Er7oKtvwW6ybX7cHM9lGGrsTnQdE8XUqXxe/fE6OuRBT3zcvhmAs0Q==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENOJ9MgCIYWnzimbQ5SAN0u5X7NbWorKEIsCmTQNTgMJ6Da4hUeHoznFB8otS9Mahw==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAELFnQtM9B0smCCoA85ovPMk+Br1IrAK2R70vHPEXUlDOkLHJMOiCrE0lbrGu2zoWTA==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317082232_AddClassType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317082232_AddClassType', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE TABLE [Equipments] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Quantity] int NOT NULL,
        [PurchaseDate] datetime2 NOT NULL,
        [PurchasePrice] decimal(18,2) NOT NULL,
        [Status] int NOT NULL,
        [Location] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Equipments] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE TABLE [Invoices] (
        [Id] uniqueidentifier NOT NULL,
        [InvoiceNumber] nvarchar(max) NOT NULL,
        [MemberId] uniqueidentifier NULL,
        [TotalAmount] decimal(18,2) NOT NULL,
        [DiscountAmount] decimal(18,2) NOT NULL,
        [PaymentMethod] int NOT NULL,
        [Status] int NOT NULL,
        [Note] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Invoices] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Invoices_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE TABLE [Orders] (
        [Id] uniqueidentifier NOT NULL,
        [OrderNumber] nvarchar(max) NOT NULL,
        [MemberId] uniqueidentifier NULL,
        [TotalAmount] decimal(18,2) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [Status] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Orders_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE TABLE [Products] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [SKU] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [StockQuantity] int NOT NULL,
        [Category] nvarchar(max) NULL,
        [ImageUrl] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE TABLE [Depreciations] (
        [Id] uniqueidentifier NOT NULL,
        [EquipmentId] uniqueidentifier NOT NULL,
        [Value] decimal(18,2) NOT NULL,
        [Date] datetime2 NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Depreciations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Depreciations_Equipments_EquipmentId] FOREIGN KEY ([EquipmentId]) REFERENCES [Equipments] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE TABLE [EquipmentTransactions] (
        [Id] uniqueidentifier NOT NULL,
        [EquipmentId] uniqueidentifier NOT NULL,
        [Type] int NOT NULL,
        [Quantity] int NOT NULL,
        [Date] datetime2 NOT NULL,
        [Note] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_EquipmentTransactions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_EquipmentTransactions_Equipments_EquipmentId] FOREIGN KEY ([EquipmentId]) REFERENCES [Equipments] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE TABLE [MaintenanceLogs] (
        [Id] uniqueidentifier NOT NULL,
        [EquipmentId] uniqueidentifier NOT NULL,
        [Date] datetime2 NOT NULL,
        [Cost] decimal(18,2) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_MaintenanceLogs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MaintenanceLogs_Equipments_EquipmentId] FOREIGN KEY ([EquipmentId]) REFERENCES [Equipments] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE TABLE [InvoiceDetails] (
        [Id] uniqueidentifier NOT NULL,
        [InvoiceId] uniqueidentifier NOT NULL,
        [ItemType] nvarchar(max) NOT NULL,
        [ReferenceId] uniqueidentifier NULL,
        [ItemName] nvarchar(max) NOT NULL,
        [Quantity] int NOT NULL,
        [UnitPrice] decimal(18,2) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_InvoiceDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_InvoiceDetails_Invoices_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE TABLE [Inventories] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        [LastUpdated] datetime2 NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Inventories] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Inventories_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE TABLE [OrderDetails] (
        [Id] uniqueidentifier NOT NULL,
        [OrderId] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_OrderDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderDetails_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderDetails_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE TABLE [StockTransactions] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Type] int NOT NULL,
        [Quantity] int NOT NULL,
        [Date] datetime2 NOT NULL,
        [Note] nvarchar(max) NULL,
        [ReferenceNumber] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_StockTransactions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_StockTransactions_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    EXEC(N'UPDATE [Payments] SET [Method] = 2
    WHERE [Id] = ''a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEIDOXjie0a27SJBkvKaDMkI306mOado58dsmoT6/K4IrMYC8h0TsaQMmpwskIWnq3w==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFXgQetnzfW+PScMXQtu6GzE8bf583Xels/cIKj6mRWe4JYcQ/zHIBBCJjpOt70enw==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEDcI3jYdLY6eRrxc0tmz3ixeIQ4m8h/PWPAthn+jhNrmdcoBxcEQ0UVpOv5rviBFyw==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEPXVASXRRccJVhNBIcB/1Qz5+EuY0VypGxRj1VRYYnaZMZ5CE5Hm3SuIakamWDUzoA==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENGbSTIzwKDWKSBGcjmbnYdOC9Ocx0jqvJri58XGw72KfIEiNODbS+7h65zgXWNs1w==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKZz+K3OtJhYMS+b+EXk2czZWujPfxm0unr+cHgf+zLFpq9oI1qeHNIa+FzGzpd/fg==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEA7WnR2VWtglpeckTH2DNdc0e/hx7msNKJIm+1j9xN7CvVJ8ETe+u44rIciwTLDLEg==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKHKrmYcJZXDwIfe6BiaDNFzlXGsTtNFke3RiA6cBWxmsUmyTStu6LSLXKgBvADrhw==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE INDEX [IX_Depreciations_EquipmentId] ON [Depreciations] ([EquipmentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE INDEX [IX_EquipmentTransactions_EquipmentId] ON [EquipmentTransactions] ([EquipmentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE INDEX [IX_Inventories_ProductId] ON [Inventories] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE INDEX [IX_InvoiceDetails_InvoiceId] ON [InvoiceDetails] ([InvoiceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE INDEX [IX_Invoices_MemberId] ON [Invoices] ([MemberId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE INDEX [IX_MaintenanceLogs_EquipmentId] ON [MaintenanceLogs] ([EquipmentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE INDEX [IX_OrderDetails_OrderId] ON [OrderDetails] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE INDEX [IX_OrderDetails_ProductId] ON [OrderDetails] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE INDEX [IX_Orders_MemberId] ON [Orders] ([MemberId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    CREATE INDEX [IX_StockTransactions_ProductId] ON [StockTransactions] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317094722_AddInventoryAndEquipment'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317094722_AddInventoryAndEquipment', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [Products] ADD [ExpirationDate] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [Products] ADD [MinStockThreshold] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [MaintenanceLogs] ADD [ScheduledDate] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [MaintenanceLogs] ADD [Status] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [MaintenanceLogs] ADD [Technician] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [EquipmentTransactions] ADD [FromLocation] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [EquipmentTransactions] ADD [ToLocation] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [Equipments] ADD [Category] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [Equipments] ADD [LastMaintenanceDate] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [Equipments] ADD [MaintenanceIntervalDays] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [Equipments] ADD [NextMaintenanceDate] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [Equipments] ADD [Priority] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [Equipments] ADD [Provider] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    ALTER TABLE [Equipments] ADD [SerialNumber] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFCqnLKXASsTCkEyIfs7IBNeqDUSx2Umg9sJPPrnU3mYegH57Ps80XPWRbszW4Hx6A==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJ8jXhr4evdrxCDqR8+QEE6BUWUQ3k2C7wYoY1ytiU+kjpXl948skzG/jzk0QKENmQ==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEO9HMw6LGVLbL2yy20u7VUYupCBsJFsuZ3mgL9FH2RN5iVnY+HU0y9Xn7qfWUguhRQ==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMd6McneFwFq28o8r4enQpiF/63/YfJMg1Tha4dw4KzeWF/HVPK22XIkdE4Unf0q/Q==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOr+EnQTI2YzsXqZA8mANjUL134OyFMELDp2eyLdjOqAoejaMaDCH6iRTLImoboEHQ==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEF+2/5kB+aXFueaaR/pzC1fqytziYoqL8n8/9bn55kRHNLBc0gr9zmVzv8dNz7Ra5g==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEL3tXlghERJtqvxTDdK/F3Kt2KHrc9/IP2eVFzYqvv7YZNXD3/iHmfeWPjoQmHNw1Q==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMV5nUGpEUs68Q4DTBljnAiNDZshW6uAjNaF6vGqY8FPJHYZSH6M+mufhfFame/uTw==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320060919_EnhanceInventoryEquipment'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260320060919_EnhanceInventoryEquipment', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    DECLARE @var6 nvarchar(max);
    SELECT @var6 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Equipments]') AND [c].[name] = N'Category');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Equipments] DROP CONSTRAINT ' + @var6 + ';');
    ALTER TABLE [Equipments] DROP COLUMN [Category];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    DECLARE @var7 nvarchar(max);
    SELECT @var7 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Equipments]') AND [c].[name] = N'Provider');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Equipments] DROP CONSTRAINT ' + @var7 + ';');
    ALTER TABLE [Equipments] DROP COLUMN [Provider];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    ALTER TABLE [Products] ADD [ProviderId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    ALTER TABLE [MaintenanceLogs] ADD [ProviderId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    ALTER TABLE [Equipments] ADD [CategoryId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    ALTER TABLE [Equipments] ADD [ProviderId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    ALTER TABLE [Equipments] ADD [WarrantyExpiryDate] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    CREATE TABLE [EquipmentCategories] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Code] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Group] nvarchar(max) NULL,
        [AvgMaintenanceCost] decimal(18,2) NULL,
        [StandardWarrantyMonths] int NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_EquipmentCategories] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    CREATE TABLE [Providers] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Code] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [Email] nvarchar(max) NULL,
        [VATCode] nvarchar(max) NULL,
        [SupplyType] nvarchar(max) NULL,
        [Note] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Providers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEPta/Nn3K/A0zEgfNdAoaMKZwB9zWowUHt2DPb/VT5lmc5eFrTG+PWfINP/hvCMx6w==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEP2227hIW4g5A3R2zL+1KWh0XMetoj3SPeoSMCAX0Gs/54+NCpPwgwn9WeYfv4XbBw==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEACTWP2D5rRLHxkBKPVg4NTD+ORelJy7b8qIiCd/JlihAewpnLXMDk9KWy7FBGC6zg==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEEPgj4PzlvVhCsMj6+t9G93Vynhmx6vbB/EnwVkGfFhNlGEeylOPV5rygA8yqvZfwA==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMi8eAEIncC6tn5Dbkn7LZDTBKcJmAnv92fWmZYg4y/idWKsPRc7EpUWbTJKCaSVyg==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEAoeqMiglGwhvUrS+oO/sHNuJidvtaBRzV2BUVSf+lRMYNIg+77Y2i+LTwQnejqhJA==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEDLxk1957LKJwrdtfQKrWWpgaAV0jQEUrETHNkavIlOOvFF0we2QxWxlHojzkYrxKQ==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAECwM1upWzARfdDXJfaw4ptuMXnJGuX0d2DqsD+47B/gFOxm/40SUnZu5GEKiKbZ0Zw==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    CREATE INDEX [IX_Products_ProviderId] ON [Products] ([ProviderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    CREATE INDEX [IX_MaintenanceLogs_ProviderId] ON [MaintenanceLogs] ([ProviderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    CREATE INDEX [IX_Equipments_CategoryId] ON [Equipments] ([CategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    CREATE INDEX [IX_Equipments_ProviderId] ON [Equipments] ([ProviderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    ALTER TABLE [Equipments] ADD CONSTRAINT [FK_Equipments_EquipmentCategories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [EquipmentCategories] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    ALTER TABLE [Equipments] ADD CONSTRAINT [FK_Equipments_Providers_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Providers] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    ALTER TABLE [MaintenanceLogs] ADD CONSTRAINT [FK_MaintenanceLogs_Providers_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Providers] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    ALTER TABLE [Products] ADD CONSTRAINT [FK_Products_Providers_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Providers] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320070402_AddCategoriesAndProviders'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260320070402_AddCategoriesAndProviders', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320071209_UpdateProviderLinks'
)
BEGIN
    ALTER TABLE [StockTransactions] ADD [ProviderId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320071209_UpdateProviderLinks'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAELiVn9PpS8wpgifBJEib6rVEc4elMwEuLVcZEEUWRMTs/f/IhKftRjh+Zo7OGpSzOA==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320071209_UpdateProviderLinks'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAECazTxytWv/Cfa3RjAKNrH3JSW15HCrnkPa2LSkQZNCJ30pKQZ/EBdsxbHANbQjxkQ==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320071209_UpdateProviderLinks'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEM1WMnhl7kAgCzYm8HG/lDHMQ5uCBn0TeKIqPiESDCixR6GrL/ZZ72SYiah3sn7CJg==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320071209_UpdateProviderLinks'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOQdLTfNJn/SgeE3gCLjsO5xSOzlUbSXNgN+R6m0XYxq+uMVmXsHUov2hLXgIYsXrQ==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320071209_UpdateProviderLinks'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEH4oDFfWABcIKm30Hiplj8tKdCovAqYCS23UWxYwnUdsWJD4C4iEjrI0vAtK4UH4pA==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320071209_UpdateProviderLinks'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOTH+ARbaPj8Kuv6lG0yLYjT6LKkRWWS92iTjCPEsk+rsmu5Vf2CUZ0j01Z2hfay4Q==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320071209_UpdateProviderLinks'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEIhu/utwb0zcpYJ7KmkcCVCNFZtgSLhldSvqJoR8E+/E8Al+bh84hAFhOsOHUr2cLA==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320071209_UpdateProviderLinks'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAECGZKYrbIvfhdzqTUYOKV7RZeWPT9XHLlMk2wUdFhQV1aqNtFrGIzh6yZjTx8sIRbw==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320071209_UpdateProviderLinks'
)
BEGIN
    CREATE INDEX [IX_StockTransactions_ProviderId] ON [StockTransactions] ([ProviderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320071209_UpdateProviderLinks'
)
BEGIN
    ALTER TABLE [StockTransactions] ADD CONSTRAINT [FK_StockTransactions_Providers_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Providers] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320071209_UpdateProviderLinks'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260320071209_UpdateProviderLinks', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    ALTER TABLE [Providers] ADD [BankAccountNumber] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    ALTER TABLE [Providers] ADD [BankName] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    ALTER TABLE [Equipments] ADD [EquipmentCode] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    CREATE TABLE [EquipmentProviderHistories] (
        [Id] uniqueidentifier NOT NULL,
        [EquipmentId] uniqueidentifier NOT NULL,
        [OldProviderId] uniqueidentifier NULL,
        [NewProviderId] uniqueidentifier NULL,
        [ChangeDate] datetime2 NOT NULL,
        [Reason] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_EquipmentProviderHistories] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_EquipmentProviderHistories_Equipments_EquipmentId] FOREIGN KEY ([EquipmentId]) REFERENCES [Equipments] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_EquipmentProviderHistories_Providers_NewProviderId] FOREIGN KEY ([NewProviderId]) REFERENCES [Providers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_EquipmentProviderHistories_Providers_OldProviderId] FOREIGN KEY ([OldProviderId]) REFERENCES [Providers] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    CREATE TABLE [IncidentLogs] (
        [Id] uniqueidentifier NOT NULL,
        [EquipmentId] uniqueidentifier NOT NULL,
        [Date] datetime2 NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Severity] nvarchar(max) NOT NULL,
        [ReportedBy] nvarchar(max) NULL,
        [ResolutionStatus] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_IncidentLogs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_IncidentLogs_Equipments_EquipmentId] FOREIGN KEY ([EquipmentId]) REFERENCES [Equipments] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKHP6gVUOPXcUlj7yDDnUV0VXkLH/Ec5nFpExFloBv76Sxa5piNZMCOkM+Im/SB2EQ==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEB/+Wh1yX8QF1Lmx7OnuUd0MX07j0Mr+umDHGMEc166pNefq7WVqD223NN9esmXBrA==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEHIfoMEjrqVAIfo41nK04/3Tv7dvuiHgQUX6eY4AvNKMcuUsF+UsEX3z+8eorAdJBQ==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEH5HT/wd52EOksPhv6Eu0x5N7nUhDebfQHEEPnVyJChp8obwlFpKza/I1XwWN4e0UA==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENFQYhLn9/yDSoI228cuDsRaW9q3XequcB5rC64jHM/fwLw4GzAV+snU44C4JuxeKA==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJ5gcFTsWrSh8Zjmy6DGL4Zv71wwkvSr2ZW5YujbAQ5X7xT133r3EJm4FhrPpG8mig==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEB86FkTjAUJJlv/2a6uF8TjvsPsmVDfxAwqrqPylMWiRp4IM3rwmdTj7a3mizj5kBA==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEIq/kfrte8c9rFKIDAxuht1Stc1LqGjTUBnOA/PUuNzFl2BCqZl1c5qc8rrEy90wGA==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    CREATE INDEX [IX_EquipmentProviderHistories_EquipmentId] ON [EquipmentProviderHistories] ([EquipmentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    CREATE INDEX [IX_EquipmentProviderHistories_NewProviderId] ON [EquipmentProviderHistories] ([NewProviderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    CREATE INDEX [IX_EquipmentProviderHistories_OldProviderId] ON [EquipmentProviderHistories] ([OldProviderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    CREATE INDEX [IX_IncidentLogs_EquipmentId] ON [IncidentLogs] ([EquipmentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320083041_UpdateEquipmentAndProviderFields'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260320083041_UpdateEquipmentAndProviderFields', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320092400_AddNewColumns_Providers_Equipments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENzARoStWlBQ9YMWcFMnf/gnMY4HugHW9/zoQpbC3lRr/N8pZcrT6H0J0Zr1iKTmLw==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320092400_AddNewColumns_Providers_Equipments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKOXqHHPQsXSo9dPO4ohwBmywDYbRSoKmKSVbL+NlTNCvltjwDvMhtRjtivJSlwyNQ==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320092400_AddNewColumns_Providers_Equipments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEH5BICOi9Cy1YGR4p8ECfDNeD6j6j/83aAcHJB7VOEIOidxnv6p3aS3/GSoP3sgZJg==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320092400_AddNewColumns_Providers_Equipments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEBaA44d8Eb5cRGb2jqQlCWMW8qPBtNDCumZNTdo/1j5Qq7ws8WFil19L6rgEoIR3KQ==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320092400_AddNewColumns_Providers_Equipments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEF4MR0HyFXJQY9aLj7L3EbVS3+UvLxmyZt8maeY9z0qmWWiPJsQ29LVpbXpZ6m3ucw==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320092400_AddNewColumns_Providers_Equipments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEIeqLMxQCApiGGrl8bTJDSfWzw+sHQhfbqLE2JSVT6CHOsiQ5YdvGI6snLVkhUn+LQ==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320092400_AddNewColumns_Providers_Equipments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAELmMsDmZhSGudT4Sq7rNJI6DHsfTCde7Q33GXD2L0fKVQ8dRj6WxduXEbQny5/xb7g==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320092400_AddNewColumns_Providers_Equipments'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOks+Rk/eEdNb2Nspr0i/LdVVDfEsLhzPxIhNwTu4KXEtLk/Z7PoODhnaumIVZq71Q==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320092400_AddNewColumns_Providers_Equipments'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260320092400_AddNewColumns_Providers_Equipments', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    EXEC sp_rename N'[Depreciations].[Value]', N'Amount', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    ALTER TABLE [Equipments] ADD [DepreciationStartDate] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    ALTER TABLE [Equipments] ADD [SalvageValue] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    ALTER TABLE [Equipments] ADD [UsefulLifeMonths] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    ALTER TABLE [Depreciations] ADD [Note] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    ALTER TABLE [Depreciations] ADD [PeriodMonth] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    ALTER TABLE [Depreciations] ADD [PeriodYear] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    ALTER TABLE [Depreciations] ADD [RemainingValue] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEO3pqjT95KxYtcO+EgQkVnJctCCkD78P11ePimx8e6p3v/9InEkN9Z0l7HPXEXTgQA==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEAlsqAG7hairCQT5i0hX9SrzexnHwvMXHjnOzeWAMKIeQBUvqw6FP8k2/NAxQd2vlg==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEF8rVEI9GdY2Wo/PmW/YBAHU9wuWd/fsiGN09xAtIhmJZLV9qbTKLg9Pfi2VBtDmuA==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEBhRGXOjQPnyvjlapF2Pi8RgtnefZnoBYNA1iiipM6vftmcDDxbqdeWfoBgxAamxlQ==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAELriaiBRKhGS74JjqmFOZrwf5d5vYQuQYh675DNt7CTI3P2ett3o2kgbDKV4/rPH9g==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFox9DKxEp65471//B6cJtFazroQZV+xQuRxnMJs/C1DFCRZYZtZll86t+RsVaW8cg==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKFYY2RVfpj4TxWF6OuOkQxHGMJywmMgbOxy0BCaIixbPlanrbDILyFLPugW14g3Jg==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMgwo4/YV+5bOaD1tp5qEpbRfTkAWA5jGgecqSZXoTc6Pc9RfI3GjkPXJKRmcDfPlQ==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141607_CompleteEquipmentDepreciationDetails'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260320141607_CompleteEquipmentDepreciationDetails', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141919_UpdateDecimalPrecision'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJJX4eCcy37xmbLfqP6J2nKnD2P+HsShv5BN0qUH38waSPWSKdLtanzXFPem4YpA6Q==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141919_UpdateDecimalPrecision'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMam6QNHoHrtRVCwlmaohFLNGH9+r/l+fkBl3IXe3qW2R0/sny45Jvs/ggO8tMdxEA==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141919_UpdateDecimalPrecision'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKmYaF7cq9tMRTKdtXWihrUS9/evRo5qGXkzPbC0FepltyCZ3dbvqYs8tsiFrx1b1A==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141919_UpdateDecimalPrecision'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFwUZq1IeLOaJOXljIFJDIAP9WULe8/1liyPKZL1DAfbKVey+hSfJNxG59jkBExaxQ==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141919_UpdateDecimalPrecision'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMiu1x74kGgmkXfFE1RrO89ZVamk+ZhQgoWtqlnsQ8YMOb35tdFwlqrMwht7jWUPqQ==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141919_UpdateDecimalPrecision'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOhvAp455sqOLdMVhP9nEB08HKdJ7BU1NOmK86BebbUKzf4Yx/Z+cncou+RsYrrqkw==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141919_UpdateDecimalPrecision'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEC58v2ZIQkOUNyTvRxo0AGEsy8HZPWrdZ0ywOv/Q7fSqrXzRidEw/06gL+DBR2D8Sg==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141919_UpdateDecimalPrecision'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKCgatpTmIhTcIap7+rm2gUsVv1txr7ljUJ6pOnTa1Tvq2yWyzEZ/IC4/hWM0Jlr9Q==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320141919_UpdateDecimalPrecision'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260320141919_UpdateDecimalPrecision', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    ALTER TABLE [StockTransactions] ADD [FromWarehouseId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    ALTER TABLE [StockTransactions] ADD [ToWarehouseId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    ALTER TABLE [StockTransactions] ADD [UnitPrice] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    ALTER TABLE [Products] ADD [MaxStockThreshold] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    ALTER TABLE [Products] ADD [Type] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    ALTER TABLE [Products] ADD [Unit] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    ALTER TABLE [Inventories] ADD [WarehouseId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    CREATE TABLE [Warehouses] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Location] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Warehouses] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMqVKfIIf8gj/tlT/ppEdSFBoJTTfnIvvI+UZoR2ZjrFvxEflXPTyt4kIvrsDlPtOw==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENzQgb6OjyS639htIHmW+0qjQd6ly2QmxyQOfhwai577vHOjlaj9NBKzzbomVb+CWw==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEG7K0fkGOV/Bsq6fa4Wrx5iiqyzKSYPgDbiVg5raXpBG1OuT9e9DyNijWBkp5BB1+w==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEGIk+b+RacP4u2TNxWKp2rDakmJB31ugFYd8FNOd84Usr/bvqf7DWp2CTa8aMHF5gQ==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAECHnAnj0fk4cyv9SDSSe3smrqcMwdFVdWxNz6MJdPDVrcOvLjBIt+QBZnJqq+8ZZYg==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEGM+5zLo9mxTOyS0MdmDg03YngaYCbZB84dYgwUrb/PBHrnSFosmIjaw/4j+ZgrqMg==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEHp2LMdOOoaGQbmKjBpYvtSc9NiCPwMx6IvPhpz7YQpQTDgbm7lbsKvuhIE4ys1ITQ==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEG1R3HsRY4kvwk/YGSgvo3Unmo/Yv+Ifx7wf77sjgOzCUEww1yobXnUPJWRd1Obvww==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'IsActive', N'IsDeleted', N'Location', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Warehouses]'))
        SET IDENTITY_INSERT [Warehouses] ON;
    EXEC(N'INSERT INTO [Warehouses] ([Id], [CreatedAt], [Description], [IsActive], [IsDeleted], [Location], [Name], [UpdatedAt])
    VALUES (''10000000-0000-0000-0000-000000000001'', ''2024-01-01T00:00:00.0000000Z'', N''Kho lưu trữ chính của phòng gym'', CAST(1 AS bit), CAST(0 AS bit), N''Tầng hầm'', N''Kho Tổng (Main)'', NULL),
    (''20000000-0000-0000-0000-000000000002'', ''2024-01-01T00:00:00.0000000Z'', N''Kho bán lẻ tại quầy tiếp khách'', CAST(1 AS bit), CAST(0 AS bit), N''Sảnh chính'', N''Quầy Lễ Tân (Counter)'', NULL),
    (''30000000-0000-0000-0000-000000000003'', ''2024-01-01T00:00:00.0000000Z'', N''Kho vật tư vận hành & vệ sinh'', CAST(1 AS bit), CAST(0 AS bit), N''Phòng kho tầng 1'', N''Kho Vật Tư (Supplies)'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'IsActive', N'IsDeleted', N'Location', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Warehouses]'))
        SET IDENTITY_INSERT [Warehouses] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    CREATE INDEX [IX_StockTransactions_FromWarehouseId] ON [StockTransactions] ([FromWarehouseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    CREATE INDEX [IX_StockTransactions_ToWarehouseId] ON [StockTransactions] ([ToWarehouseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    CREATE INDEX [IX_Inventories_WarehouseId] ON [Inventories] ([WarehouseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    ALTER TABLE [Inventories] ADD CONSTRAINT [FK_Inventories_Warehouses_WarehouseId] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouses] ([Id]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    ALTER TABLE [StockTransactions] ADD CONSTRAINT [FK_StockTransactions_Warehouses_FromWarehouseId] FOREIGN KEY ([FromWarehouseId]) REFERENCES [Warehouses] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    ALTER TABLE [StockTransactions] ADD CONSTRAINT [FK_StockTransactions_Warehouses_ToWarehouseId] FOREIGN KEY ([ToWarehouseId]) REFERENCES [Warehouses] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320151426_ExpandInventoryModule'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260320151426_ExpandInventoryModule', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    ALTER TABLE [Products] ADD [Barcode] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    ALTER TABLE [Products] ADD [CostPrice] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    ALTER TABLE [Products] ADD [DurationDays] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    ALTER TABLE [Products] ADD [SessionCount] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    ALTER TABLE [Products] ADD [TrackInventory] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEL2wvjjkHqax12R0IeO+ODguHwmli6yrLHVA8J5m0CTDh62GaVnX4AQ+x6vZkHoIKQ==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEDHh7OV6lje7fBgaONQUYkXCpquRud6tPk5tWUC5M5ECtPS5lonuv68WBkVpp09ZaQ==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEM9tZAp/3jtyHi4NU2+/KgMF/58Lh7T9ouqV5OTPzcW08vJMcHzeBlbpMjiSQD6nYw==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEE40F2WdhaXvs8Ety3v5yZT9TFxlTki2Jj5w3TRjGDtUbXG48GCyYk7sTYVOo4YajA==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAECAqoY8uA+czJoMVALmMSoK0+qgOnK0kybFPALlaKbe3UfiyvA/YKAnwnvJzCmfq9w==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEPuOhyq8sS7QOf5JWNv81Hu/vUF5zj2EiZjp4DediwXtI5B0dJ4/R7xMlg3iXdmFjw==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEELveAsQ3SY71Q/n1j8zsJsecQmn2VKh1VXCwfgoS6B47dZm1neH7jLEztv9lJfX+w==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEHI50zn4zIL6NRibpYN/CuMZbWkiER7zgY7M0Mk6y2Y1t0qY1xKBT75rDFw3zlsvoQ==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260320153607_AddProductModuleDetails'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260320153607_AddProductModuleDetails', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    EXEC(N'DELETE FROM [Warehouses]
    WHERE [Id] = ''30000000-0000-0000-0000-000000000003'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    ALTER TABLE [StockTransactions] ADD [AfterQuantity] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    ALTER TABLE [StockTransactions] ADD [BeforeQuantity] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    ALTER TABLE [StockTransactions] ADD [PerformedBy] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    ALTER TABLE [EquipmentTransactions] ADD [AfterQuantity] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    ALTER TABLE [EquipmentTransactions] ADD [BeforeQuantity] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    ALTER TABLE [EquipmentTransactions] ADD [CreatedBy] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    ALTER TABLE [Equipments] ADD [AccumulatedDepreciation] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    ALTER TABLE [Equipments] ADD [IsFullyDepreciated] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    ALTER TABLE [Equipments] ADD [MonthlyDepreciationAmount] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    ALTER TABLE [Equipments] ADD [RemainingValue] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    CREATE TABLE [MaintenanceMaterials] (
        [Id] uniqueidentifier NOT NULL,
        [MaintenanceLogId] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        [UnitPrice] decimal(18,2) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_MaintenanceMaterials] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MaintenanceMaterials_MaintenanceLogs_MaintenanceLogId] FOREIGN KEY ([MaintenanceLogId]) REFERENCES [MaintenanceLogs] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_MaintenanceMaterials_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEBxG6S5l20B2txCZWP+/JNbS/XHRlZL5CfZsv9FQFlOt1k6vKr+H05YA1j+NoWsIqw==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEG3CyccCA8p+Hp2Cm1RauhqEPjoWWlh32qyIJ2W/nn3MofVxeFIdnS3Q13k7T7+3oA==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEAIIdDt9Q7bWqmFmhGGceQkAK4OydOYSeDlvXD55ZhUjpgSVIwtmWj+gM1LEp3KLEw==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAED17rIwkm0NgswWKXpFIyINgjksG3D0s/00jJMtZnEMsWrw0vvEAEUWxlw3NKL2V6w==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEGyODp0ZpkaE0ivoktrjr2/Yh4SMV+YP9TOikH2nSGXF52H9ZsRvoLZTqZYrA0Cmyg==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEK/C3WPEh3ELM7KaURHgCCaUqGrW283IdJ5wW/6ZKVPaijV5wlB4h6jeLWGzBGs3Dw==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJMJOVkmrvdtfniAnpQNH1JKICyiWojXF4O+iHkBlOiiFgrYymTNcPQ4xsr+kX/CHA==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOMxsF1+mNFwLGgZErJrYiRyvvmDnqKxW3BLqHnIgPeUXFsTOPfW7cGsgwGsGQowMA==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    EXEC(N'UPDATE [Warehouses] SET [Description] = N''Kho nhập hàng lớn và lưu trữ chính''
    WHERE [Id] = ''10000000-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    EXEC(N'UPDATE [Warehouses] SET [Description] = N''Kho bán lẻ & vật tư tại quầy'', [Name] = N''Kho Quầy''
    WHERE [Id] = ''20000000-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    CREATE INDEX [IX_MaintenanceMaterials_MaintenanceLogId] ON [MaintenanceMaterials] ([MaintenanceLogId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    CREATE INDEX [IX_MaintenanceMaterials_ProductId] ON [MaintenanceMaterials] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321124329_AddMissingInventoryAndAssetFields'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260321124329_AddMissingInventoryAndAssetFields', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321141404_AddEquipmentWeight'
)
BEGIN
    ALTER TABLE [Equipments] ADD [Weight] float NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321141404_AddEquipmentWeight'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEAiPRewCheaWJ28O+VoFTuxkyd+/RBTNSyc+fJUwJ7ybX4HTcnMlishvtvtm3PpRLw==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321141404_AddEquipmentWeight'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEBnraX7+c0FOE1cSLk9uGh6RlSY07iKfBnHqPkQLxfho1Ej1zRbYEL+zsa+exPp8/g==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321141404_AddEquipmentWeight'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOxdqFx1070h3dUhgK0gRPluD7+77Y8R1WcHJ37aIapQCRJJHE/vPh/3TxtfOsesUQ==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321141404_AddEquipmentWeight'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAELDj0c9+qyoCkSsGbo94LdTJiWWT1OZgG9nTv2895za0ry/MgqnGMmAkGC/VsnlMxw==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321141404_AddEquipmentWeight'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEOdcVz4SKI1pGRWB5uLlei7AdTBBUtOkAKJw+p1BvmKgVUStCtxd8i6jcR2nh1CNnA==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321141404_AddEquipmentWeight'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMHdAX2qmn/geIemc0l8AVsXo/QfoNifRj2Ekc9t4D+h/QMdPipM7wU6u4dM4sQ1gA==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321141404_AddEquipmentWeight'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKDKka5kuaQAQfeHqRb2WPI5oz/zUSpMfSpQkcYdv6hc8xcDTwjaT6MXLGL9958WvQ==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321141404_AddEquipmentWeight'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEDTEeJiN2JYMgREI1qdnqRANJO+AmGsU6iUcLrlTd2gHsUA6S2EdM/dHPnmjvz+cYg==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260321141404_AddEquipmentWeight'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260321141404_AddEquipmentWeight', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    ALTER TABLE [EquipmentProviderHistories] DROP CONSTRAINT [FK_EquipmentProviderHistories_Providers_NewProviderId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    ALTER TABLE [EquipmentProviderHistories] DROP CONSTRAINT [FK_EquipmentProviderHistories_Providers_OldProviderId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    ALTER TABLE [Invoices] DROP CONSTRAINT [FK_Invoices_Members_MemberId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    ALTER TABLE [OrderDetails] DROP CONSTRAINT [FK_OrderDetails_Products_ProductId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    ALTER TABLE [Users] DROP CONSTRAINT [FK_Users_Roles_RoleId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    DROP INDEX [IX_Users_RoleId] ON [Users];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    DECLARE @var8 nvarchar(max);
    SELECT @var8 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'RoleId');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT ' + @var8 + ';');
    ALTER TABLE [Users] DROP COLUMN [RoleId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC sp_rename N'[Providers].[VATCode]', N'TaxCode', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    DECLARE @var9 nvarchar(max);
    SELECT @var9 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Providers]') AND [c].[name] = N'Code');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Providers] DROP CONSTRAINT ' + @var9 + ';');
    ALTER TABLE [Providers] ALTER COLUMN [Code] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    ALTER TABLE [Providers] ADD [ContactPerson] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    DECLARE @var10 nvarchar(max);
    SELECT @var10 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'SKU');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT ' + @var10 + ';');
    ALTER TABLE [Products] ALTER COLUMN [SKU] nvarchar(50) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    DECLARE @var11 nvarchar(max);
    SELECT @var11 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MemberSubscriptions]') AND [c].[name] = N'OriginalPackageName');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [MemberSubscriptions] DROP CONSTRAINT ' + @var11 + ';');
    ALTER TABLE [MemberSubscriptions] ALTER COLUMN [OriginalPackageName] nvarchar(255) NOT NULL;
    ALTER TABLE [MemberSubscriptions] ADD DEFAULT N'' FOR [OriginalPackageName];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    DECLARE @var12 nvarchar(max);
    SELECT @var12 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Members]') AND [c].[name] = N'QRCode');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Members] DROP CONSTRAINT ' + @var12 + ';');
    ALTER TABLE [Members] ALTER COLUMN [QRCode] nvarchar(255) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    DECLARE @var13 nvarchar(max);
    SELECT @var13 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Invoices]') AND [c].[name] = N'InvoiceNumber');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Invoices] DROP CONSTRAINT ' + @var13 + ';');
    ALTER TABLE [Invoices] ALTER COLUMN [InvoiceNumber] nvarchar(50) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    DECLARE @var14 nvarchar(max);
    SELECT @var14 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[InvoiceDetails]') AND [c].[name] = N'ItemName');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [InvoiceDetails] DROP CONSTRAINT ' + @var14 + ';');
    ALTER TABLE [InvoiceDetails] ALTER COLUMN [ItemName] nvarchar(255) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    CREATE TABLE [UserRoles] (
        [UserId] uniqueidentifier NOT NULL,
        [RoleId] int NOT NULL,
        [AssignedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AvgMaintenanceCost', N'Code', N'CreatedAt', N'Description', N'Group', N'IsDeleted', N'Name', N'StandardWarrantyMonths', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[EquipmentCategories]'))
        SET IDENTITY_INSERT [EquipmentCategories] ON;
    EXEC(N'INSERT INTO [EquipmentCategories] ([Id], [AvgMaintenanceCost], [Code], [CreatedAt], [Description], [Group], [IsDeleted], [Name], [StandardWarrantyMonths], [UpdatedAt])
    VALUES (''f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1'', 200000.0, N'''', ''2024-01-01T00:00:00.0000000Z'', N''Các loại máy chạy bộ, xe đạp, trượt tuyết'', NULL, CAST(0 AS bit), N''Máy Cardio'', NULL, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AvgMaintenanceCost', N'Code', N'CreatedAt', N'Description', N'Group', N'IsDeleted', N'Name', N'StandardWarrantyMonths', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[EquipmentCategories]'))
        SET IDENTITY_INSERT [EquipmentCategories] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Address', N'BankAccountNumber', N'BankName', N'Code', N'ContactPerson', N'CreatedAt', N'Email', N'IsActive', N'IsDeleted', N'Name', N'Note', N'PhoneNumber', N'SupplyType', N'TaxCode', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Providers]'))
        SET IDENTITY_INSERT [Providers] ON;
    EXEC(N'INSERT INTO [Providers] ([Id], [Address], [BankAccountNumber], [BankName], [Code], [ContactPerson], [CreatedAt], [Email], [IsActive], [IsDeleted], [Name], [Note], [PhoneNumber], [SupplyType], [TaxCode], [UpdatedAt])
    VALUES (''e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1'', N''123 Đường số 7, TP.HCM'', NULL, NULL, NULL, N''Nguyễn Văn Cung'', ''2024-01-01T00:00:00.0000000Z'', N''contact@gymglobal.com'', CAST(1 AS bit), CAST(0 AS bit), N''Công ty Thiết bị Gym Toàn Cầu'', NULL, N''02838445566'', NULL, N''0314567890'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Address', N'BankAccountNumber', N'BankName', N'Code', N'ContactPerson', N'CreatedAt', N'Email', N'IsActive', N'IsDeleted', N'Name', N'Note', N'PhoneNumber', N'SupplyType', N'TaxCode', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Providers]'))
        SET IDENTITY_INSERT [Providers] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [Description] = N''Quản lý chủ gym - Toàn quyền''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [Description] = N''Quản lý vận hành'', [Permissions] = N''["members.*", "packages.*", "subscriptions.*", "payments.*", "checkins.*", "reports.*", "inventory.*", "equipment.*", "billing.*"]''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [Description] = N''HLV quản lý lớp và báo hỏng thiết bị'', [Permissions] = N''["class.manage", "equipment.report", "inventory.consume", "member.read"]''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [Description] = N''Nhân viên lễ tân vận hành hàng ngày'', [Permissions] = N''["checkin.create", "member.read", "member.create", "member.update", "subscription.read", "subscription.create", "inventory.consume", "report.read"]''
    WHERE [Id] = 4;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'Permissions', N'RoleName') AND [object_id] = OBJECT_ID(N'[Roles]'))
        SET IDENTITY_INSERT [Roles] ON;
    EXEC(N'INSERT INTO [Roles] ([Id], [CreatedAt], [Description], [Permissions], [RoleName])
    VALUES (5, ''2024-01-01T00:00:00.0000000Z'', N''Hội viên tự phục vụ'', N''["checkin.self", "member.profile.read", "subscription.read", "class.enroll"]'', N''Member'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'Permissions', N'RoleName') AND [object_id] = OBJECT_ID(N'[Roles]'))
        SET IDENTITY_INSERT [Roles] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[UserRoles]'))
        SET IDENTITY_INSERT [UserRoles] ON;
    EXEC(N'INSERT INTO [UserRoles] ([RoleId], [UserId], [AssignedAt])
    VALUES (1, ''11111111-1111-1111-1111-111111111111'', ''2026-03-22T08:43:01.6513504Z''),
    (2, ''22222222-2222-2222-2222-222222222222'', ''2026-03-22T08:43:01.6528034Z''),
    (3, ''33333333-3333-3333-3333-333333333333'', ''2026-03-22T08:43:01.6528062Z''),
    (3, ''44444444-4444-4444-4444-444444444444'', ''2026-03-22T08:43:01.6528068Z''),
    (4, ''55555555-5555-5555-5555-555555555555'', ''2026-03-22T08:43:01.6528074Z'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[UserRoles]'))
        SET IDENTITY_INSERT [UserRoles] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEMrrmqY7il+mVPSfzKvWYwwn6jyyJIO9j+0RjaAJXP6EdWo3+rdoDwF/PQOyvknuVw==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKLeEO7/FJ5SJBvXWiwIvObQ+fonizVgkoXxxnsEHiUnkFOANQu8HmZS3URiyz6ZvA==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEC/0Z2o1lKv1M3MofbqIQlnSyF6bvIMunuByfR8eNYQZ4qSteOenc5PPkFvBNtjBxA==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKzfTdca/IKsSq7os+5SO7KfKdE+/W9pqBrJwCwIQdLCw3vOfk4gi23SaogfmTz38g==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEDzoR3qrlPFgeOziV8yiUZgaEpJ8/qWkgwHncFPdHwpBznybMhqwBHrPUJq+JGBwsQ==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [CreatedAt] = ''2024-01-01T00:00:00.0000000Z'', [PasswordHash] = N''AQAAAAIAAYagAAAAEC1a+Pn1Ii+PE19ofALeUCtQXWx7RED+DVEgFfmL929ag1jo/iLchssIX3kz/8Gjyw==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [CreatedAt] = ''2024-01-01T00:00:00.0000000Z'', [PasswordHash] = N''AQAAAAIAAYagAAAAEOa4HfRKa+2HRh3+OT7apWLu9nYcuUmjw1jafbV9tn5uT75wPyYIk0u3wtAnYO+71w==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [CreatedAt] = ''2024-01-01T00:00:00.0000000Z'', [Email] = N''levanc_member@gmail.com'', [FullName] = N''Lê Văn C (Member)'', [PasswordHash] = N''AQAAAAIAAYagAAAAEKkNajLEIctqwbAP04xPfpmFNRXvX6NEYN8DiWmvgj/x9dVh1eKJrvDTJIKDPPk6Sw==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'FullName', N'IsActive', N'LastLoginAt', N'PasswordHash', N'PhoneNumber', N'UpdatedAt', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] ON;
    EXEC(N'INSERT INTO [Users] ([Id], [CreatedAt], [Email], [FullName], [IsActive], [LastLoginAt], [PasswordHash], [PhoneNumber], [UpdatedAt], [Username])
    VALUES (''99999999-9999-9999-9999-999999999999'', ''2024-01-01T00:00:00.0000000Z'', N''levanc@gym.com'', N''Lê Văn C'', CAST(1 AS bit), NULL, N''AQAAAAIAAYagAAAAEDzXsvG/6MmwSDiXLTnkX4g/EHhUm4aavZQVZZheJbenCpLdkdyZXMSZsELzRwfL3g=='', N''0909999999'', NULL, N''levanc'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'FullName', N'IsActive', N'LastLoginAt', N'PasswordHash', N'PhoneNumber', N'UpdatedAt', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccumulatedDepreciation', N'CategoryId', N'CreatedAt', N'DepreciationStartDate', N'Description', N'EquipmentCode', N'IsDeleted', N'IsFullyDepreciated', N'LastMaintenanceDate', N'Location', N'MaintenanceIntervalDays', N'MonthlyDepreciationAmount', N'Name', N'NextMaintenanceDate', N'Priority', N'ProviderId', N'PurchaseDate', N'PurchasePrice', N'Quantity', N'RemainingValue', N'SalvageValue', N'SerialNumber', N'Status', N'UpdatedAt', N'UsefulLifeMonths', N'WarrantyExpiryDate', N'Weight') AND [object_id] = OBJECT_ID(N'[Equipments]'))
        SET IDENTITY_INSERT [Equipments] ON;
    EXEC(N'INSERT INTO [Equipments] ([Id], [AccumulatedDepreciation], [CategoryId], [CreatedAt], [DepreciationStartDate], [Description], [EquipmentCode], [IsDeleted], [IsFullyDepreciated], [LastMaintenanceDate], [Location], [MaintenanceIntervalDays], [MonthlyDepreciationAmount], [Name], [NextMaintenanceDate], [Priority], [ProviderId], [PurchaseDate], [PurchasePrice], [Quantity], [RemainingValue], [SalvageValue], [SerialNumber], [Status], [UpdatedAt], [UsefulLifeMonths], [WarrantyExpiryDate], [Weight])
    VALUES (''d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1'', 0.0, ''f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1'', ''2024-01-01T00:00:00.0000000Z'', NULL, NULL, N''EQP-RUN-001'', CAST(0 AS bit), CAST(0 AS bit), NULL, N''Khu Cardio Tầng 1'', 90, 333333.0, N''Máy chạy bộ Matrix T7'', NULL, 3, ''e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1'', ''2024-01-01T00:00:00.0000000'', 25000000.0, 5, 25000000.0, 5000000.0, N''MTX7-2024-X1'', 1, NULL, 60, NULL, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccumulatedDepreciation', N'CategoryId', N'CreatedAt', N'DepreciationStartDate', N'Description', N'EquipmentCode', N'IsDeleted', N'IsFullyDepreciated', N'LastMaintenanceDate', N'Location', N'MaintenanceIntervalDays', N'MonthlyDepreciationAmount', N'Name', N'NextMaintenanceDate', N'Priority', N'ProviderId', N'PurchaseDate', N'PurchasePrice', N'Quantity', N'RemainingValue', N'SalvageValue', N'SerialNumber', N'Status', N'UpdatedAt', N'UsefulLifeMonths', N'WarrantyExpiryDate', N'Weight') AND [object_id] = OBJECT_ID(N'[Equipments]'))
        SET IDENTITY_INSERT [Equipments] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Barcode', N'Category', N'CostPrice', N'CreatedAt', N'Description', N'DurationDays', N'ExpirationDate', N'ImageUrl', N'IsActive', N'IsDeleted', N'MaxStockThreshold', N'MinStockThreshold', N'Name', N'Price', N'ProviderId', N'SKU', N'SessionCount', N'StockQuantity', N'TrackInventory', N'Type', N'Unit', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Products]'))
        SET IDENTITY_INSERT [Products] ON;
    EXEC(N'INSERT INTO [Products] ([Id], [Barcode], [Category], [CostPrice], [CreatedAt], [Description], [DurationDays], [ExpirationDate], [ImageUrl], [IsActive], [IsDeleted], [MaxStockThreshold], [MinStockThreshold], [Name], [Price], [ProviderId], [SKU], [SessionCount], [StockQuantity], [TrackInventory], [Type], [Unit], [UpdatedAt])
    VALUES (''91919191-9191-9191-9191-919191919191'', NULL, N''Đồ uống'', 5000.0, ''2024-01-01T00:00:00.0000000Z'', N''Nước khoáng thiên nhiên'', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), 200, 20, N''Nước suối Lavie 500ml'', 10000.0, ''e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1'', N''WTR-001'', NULL, 100, CAST(1 AS bit), 3, N''Chai'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Barcode', N'Category', N'CostPrice', N'CreatedAt', N'Description', N'DurationDays', N'ExpirationDate', N'ImageUrl', N'IsActive', N'IsDeleted', N'MaxStockThreshold', N'MinStockThreshold', N'Name', N'Price', N'ProviderId', N'SKU', N'SessionCount', N'StockQuantity', N'TrackInventory', N'Type', N'Unit', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Products]'))
        SET IDENTITY_INSERT [Products] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[UserRoles]'))
        SET IDENTITY_INSERT [UserRoles] ON;
    EXEC(N'INSERT INTO [UserRoles] ([RoleId], [UserId], [AssignedAt])
    VALUES (5, ''66666666-6666-6666-6666-666666666666'', ''2026-03-22T08:43:01.6528154Z''),
    (5, ''77777777-7777-7777-7777-777777777777'', ''2026-03-22T08:43:01.6528159Z''),
    (5, ''88888888-8888-8888-8888-888888888888'', ''2026-03-22T08:43:01.6528164Z''),
    (3, ''99999999-9999-9999-9999-999999999999'', ''2026-03-22T08:43:01.6528149Z''),
    (4, ''99999999-9999-9999-9999-999999999999'', ''2026-03-22T08:43:01.6528080Z'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[UserRoles]'))
        SET IDENTITY_INSERT [UserRoles] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Products_SKU] ON [Products] ([SKU]) WHERE [IsDeleted] = 0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Invoices_InvoiceNumber] ON [Invoices] ([InvoiceNumber]) WHERE [IsDeleted] = 0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    ALTER TABLE [EquipmentProviderHistories] ADD CONSTRAINT [FK_EquipmentProviderHistories_Providers_NewProviderId] FOREIGN KEY ([NewProviderId]) REFERENCES [Providers] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    ALTER TABLE [EquipmentProviderHistories] ADD CONSTRAINT [FK_EquipmentProviderHistories_Providers_OldProviderId] FOREIGN KEY ([OldProviderId]) REFERENCES [Providers] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    ALTER TABLE [Invoices] ADD CONSTRAINT [FK_Invoices_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE SET NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    ALTER TABLE [OrderDetails] ADD CONSTRAINT [FK_OrderDetails_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322084309_RBAC_Implementation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260322084309_RBAC_Implementation', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [MembershipPackages] SET [HasPT] = CAST(0 AS bit)
    WHERE [Id] = ''10101010-1010-1010-1010-101010101010'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [MembershipPackages] SET [HasPT] = CAST(0 AS bit)
    WHERE [Id] = ''cccccccc-cccc-cccc-cccc-cccccccccccc'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [MembershipPackages] SET [HasPT] = CAST(0 AS bit)
    WHERE [Id] = ''dddddddd-dddd-dddd-dddd-dddddddddddd'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [MembershipPackages] SET [HasPT] = CAST(0 AS bit)
    WHERE [Id] = ''eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [MembershipPackages] SET [HasPT] = CAST(0 AS bit)
    WHERE [Id] = ''ffffffff-ffff-ffff-ffff-ffffffffffff'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [Description] = N''👑 Admin/Manager (Quản lý chủ gym) - TOÀN QUYỀN''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [Description] = N''Tiểu quản lý vận hành'', [Permissions] = N''["*"]''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [Description] = N''🏋️ Trainer - Chuyên môn'', [Permissions] = N''["member.read","class.read","class.manage","equipment.read","equipment.report","inventory.consume"]''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [Description] = N''👩💼 Receptionist - Vận hành hàng ngày'', [Permissions] = N''["member.read","member.create","member.update","checkin.create","checkin.read","package.read","inventory.read","inventory.consume","report.read"]''
    WHERE [Id] = 4;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [Description] = N''👤 Member - Tự phục vụ'', [Permissions] = N''["member.read","checkin.create","class.read"]''
    WHERE [Id] = 5;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:39:51.6032727Z''
    WHERE [RoleId] = 1 AND [UserId] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:39:51.6033603Z''
    WHERE [RoleId] = 2 AND [UserId] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:39:51.6033605Z''
    WHERE [RoleId] = 3 AND [UserId] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:39:51.6033606Z''
    WHERE [RoleId] = 3 AND [UserId] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:39:51.6033607Z''
    WHERE [RoleId] = 4 AND [UserId] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:39:51.6033616Z''
    WHERE [RoleId] = 5 AND [UserId] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:39:51.6033618Z''
    WHERE [RoleId] = 5 AND [UserId] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:39:51.6033619Z''
    WHERE [RoleId] = 5 AND [UserId] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:39:51.6033615Z''
    WHERE [RoleId] = 3 AND [UserId] = ''99999999-9999-9999-9999-999999999999'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:39:51.6033608Z''
    WHERE [RoleId] = 4 AND [UserId] = ''99999999-9999-9999-9999-999999999999'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEF1gjyXoif09w84iyQRROxsukYCveXpFAB0S99sp3/lhqJZRJqT7Qj3tOLrAEju6zw==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEEhbdHKBThGiRqpgRyiRQF/I/fvDQjzUaxXTohxbvwqfdpWMvx7NR38RAafME8oEjw==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKBf1Ntua4GObrmTPfnSrnHooaxqAKXFBGEbOeOipVkHE96t1GqSNQTflucObYdP2g==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAELnntOhyhrb3mrcTOk00bwkLJdI6uV0by8DK04GtsH2lQXGN6Hd8+MfKtYJTOj6FVw==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEKUv51+RWnCNd3Jbx6yMEo8r7AWUYMKvNj1XdWapP09/ktrtBkx8dq0aL2x8FKlrRA==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEBrrHXAvj8gAPuWggULBUnGa4zG5U6oCto5BlZBsvjjlZblIPwD7oLfqWOpo8pIHvQ==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAENOzXUl7ARpVabc9r7n0y2qEqgIf2B1FvXIR8RIA/ZxdLx7BiFrZ1WbvBdIOZbTtKw==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEFnAHo8vTK+SJc/CStD1tsMCQgysKVVLKJB1Fk/Vi5aOc8/f/oZ2j71O6LseQq/KvQ==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEBZo9rL3k2+/w/J8ekN/YA+LE/5SxFqxRNovl8qiW86KudW8jLyIg8zASuefTlyJpA==''
    WHERE [Id] = ''99999999-9999-9999-9999-999999999999'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323103957_Sync_All_Modules'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260323103957_Sync_All_Modules', N'10.0.2');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:46:59.5354630Z''
    WHERE [RoleId] = 1 AND [UserId] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:46:59.5355879Z''
    WHERE [RoleId] = 2 AND [UserId] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:46:59.5355881Z''
    WHERE [RoleId] = 3 AND [UserId] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:46:59.5355883Z''
    WHERE [RoleId] = 3 AND [UserId] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:46:59.5355885Z''
    WHERE [RoleId] = 4 AND [UserId] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:46:59.5355902Z''
    WHERE [RoleId] = 5 AND [UserId] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:46:59.5355903Z''
    WHERE [RoleId] = 5 AND [UserId] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:46:59.5355905Z''
    WHERE [RoleId] = 5 AND [UserId] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:46:59.5355900Z''
    WHERE [RoleId] = 3 AND [UserId] = ''99999999-9999-9999-9999-999999999999'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-03-23T10:46:59.5355887Z''
    WHERE [RoleId] = 4 AND [UserId] = ''99999999-9999-9999-9999-999999999999'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJYQVvAk5hSxQO2/RXC7hdUYAKIdRWzZnhYoyljHmJ87o6XlWDASBIPnG+8e/oBo5Q==''
    WHERE [Id] = ''11111111-1111-1111-1111-111111111111'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJwTsSQW5H+tNS/9gs5Ay5VeQiX/GITboTWNukqqJrhjvc5VqpMHn2XimTJfUMN/DA==''
    WHERE [Id] = ''22222222-2222-2222-2222-222222222222'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEP0kDnz6bYM//oD9A0njMLV6PVDti3U1zKpjL61/GNL/prcutuIYv/lGNx+84CK57A==''
    WHERE [Id] = ''33333333-3333-3333-3333-333333333333'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEGCN2UQXW5KyxhhVrZbmUTdul1YPiDvA2Bd80ayZhWmcm2lSyBnc8wE0Z2x92ZM/NQ==''
    WHERE [Id] = ''44444444-4444-4444-4444-444444444444'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJuXDXa4wvqT1qc0MpDwRb0t4tKaBHF2HehjOSP/HN2DLZsilZ+Y1jfbDytQbOzKLw==''
    WHERE [Id] = ''55555555-5555-5555-5555-555555555555'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEJsUHLCdW1D8mqhDLczGXRmTmv77KcesLEopf6llKzbYShrbhWBkTCF3GfYylFj0bQ==''
    WHERE [Id] = ''66666666-6666-6666-6666-666666666666'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEG6U0yemIvxOcTRON6V+vsuzXKV0t/T60hna2Pm77T6tonWvCZgr4V6+5vnnjgkGMA==''
    WHERE [Id] = ''77777777-7777-7777-7777-777777777777'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEHtafHYu38/8vE+rMSU/lf9bCpghKcYjmMwFC/RagvVp9NZpoBFfg1w9dUs1Eauueg==''
    WHERE [Id] = ''88888888-8888-8888-8888-888888888888'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [PasswordHash] = N''AQAAAAIAAYagAAAAEPZf226BFs7zDOGaY3k4k4ZlEhYdYsq6xL3KLGQSJETIkJMJZmZhUg19gLmQvDvF3w==''
    WHERE [Id] = ''99999999-9999-9999-9999-999999999999'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260323104701_FinalCheck'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260323104701_FinalCheck', N'10.0.2');
END;

COMMIT;
GO

