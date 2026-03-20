using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MembershipPackages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    DurationInMonths = table.Column<int>(type: "int", nullable: false),
                    SessionLimit = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPackages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MemberSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RemainingSessions = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberSubscriptions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberSubscriptions_MembershipPackages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "MembershipPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleDay = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    MaxCapacity = table.Column<int>(type: "int", nullable: false),
                    CurrentEnrollment = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckIns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckIns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckIns_MemberSubscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "MemberSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CheckIns_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberSubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Method = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_MemberSubscriptions_MemberSubscriptionId",
                        column: x => x.MemberSubscriptionId,
                        principalTable: "MemberSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassEnrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnrolledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassEnrollments_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassEnrollments_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MembershipPackages",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPrice", "DurationInDays", "DurationInMonths", "IsActive", "IsDeleted", "Name", "Price", "SessionLimit", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10101010-1010-1010-1010-101010101010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Toàn bộ dịch vụ + PT cá nhân trong 365 ngày", 4000000m, 365, 12, true, false, "Gói VIP 1 Năm", 4500000m, null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Truy cập phòng gym không giới hạn trong 30 ngày", null, 30, 1, true, false, "Gói Cơ Bản 1 Tháng", 500000m, null, null },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gym + Tất cả lớp học trong 30 ngày", 750000m, 30, 1, true, false, "Gói Premium 1 Tháng", 800000m, null, null },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "10 buổi tập, hạn sử dụng 60 ngày", null, 60, 2, true, false, "Gói 10 Buổi Tập", 450000m, 10, null },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Truy cập không giới hạn 90 ngày", 1100000m, 90, 3, true, false, "Gói 3 Tháng", 1200000m, null, null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Permissions", "RoleName" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Full system access", "[\"*\"]", "Admin" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manage gym operations", "[\"members.*\", \"packages.*\", \"subscriptions.*\", \"payments.*\", \"checkins.*\", \"reports.*\"]", "Manager" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manage classes and view members", "[\"classes.*\", \"members.view\", \"checkins.view\"]", "Trainer" },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Check-in and basic operations", "[\"checkins.*\", \"members.view\", \"subscriptions.view\"]", "Receptionist" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "PasswordHash", "PhoneNumber", "RoleId", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@gym.com", "System Administrator", true, null, "AQAAAAIAAYagAAAAEHfWXyU6uWwVadi4T6ReS+A/vmwhKgzPJd8YpV+aggDy1qy1YIaR0T2RO47MGCTvmA==", "0901234567", 1, null, "admin" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "manager@gym.com", "Nguyễn Văn Manager", true, null, "AQAAAAIAAYagAAAAEKKm+j5Nf5ANxzCqhDAov7WsTuAdZQyEUTZ4skYTe+D/33eoc+bEjTBq5CjufVaDQw==", "0902345678", 2, null, "manager" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "trainer1@gym.com", "Trần Thị Hương", true, null, "AQAAAAIAAYagAAAAEFEZlnu4vjmcKE8n+qmWHcuS5nomk7qm3ie5PA5//9Hk1sDEl1ckdKeEwmds7LAkkg==", "0903456789", 3, null, "trainer1" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "trainer2@gym.com", "Lê Văn Nam", true, null, "AQAAAAIAAYagAAAAEFvDOa/gWzRHNEMRxQfbyEkkYoH4ACf+VE2msJUwVbhzopOr/z5QtMqzCJgWcDMyqg==", "0904567890", 3, null, "trainer2" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "receptionist@gym.com", "Phạm Thị Lan", true, null, "AQAAAAIAAYagAAAAEEwTNWgV1DHgqWT06Lid9xxbihZ7eSiG310YqahSYdodG/xuJr/38+YnTk6jF26Acw==", "0905678901", 4, null, "receptionist" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nguyenvana@gmail.com", "Nguyễn Văn A", true, null, "AQAAAAIAAYagAAAAEN8DAiv8ruDWJvQ67OXf5fsONBX1DHMu4+qALVxE8XhK26K+qd1qhbjQUzKzWTnGDQ==", "0906789012", 4, null, "member001" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), "tranthib@gmail.com", "Trần Thị B", true, null, "AQAAAAIAAYagAAAAEED9fbsv8ynj2PQ6PtzZoLNyPkINoi+TJTbhqzSN7bIAqcRsGcyrQNg3PbvmRz8hgw==", "0907890123", 4, null, "member002" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "levanc@gmail.com", "Lê Văn C", true, null, "AQAAAAIAAYagAAAAEBkIow4as09pKB/NjDUj5ACl49ZjytnokL0JctDQvFb0WV0Z6qy21I5GYBYvBgtYkQ==", "0908901234", 4, null, "member003" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "Email", "FirstName", "IsDeleted", "JoinedDate", "LastName", "MemberCode", "PhoneNumber", "Status", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("20202020-2020-2020-2020-202020202020"), new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1995, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "nguyenvana@gmail.com", "Văn A", false, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nguyễn", "GYM2024001", "0906789012", 1, null, new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("30303030-3030-3030-3030-303030303030"), new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1992, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "tranthib@gmail.com", "Thị B", false, new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Trần", "GYM2024002", "0907890123", 1, null, new Guid("77777777-7777-7777-7777-777777777777") },
                    { new Guid("40404040-4040-4040-4040-404040404040"), new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1998, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "levanc@gmail.com", "Văn C", false, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Lê", "GYM2024003", "0908901234", 1, null, new Guid("88888888-8888-8888-8888-888888888888") }
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "Bio", "CreatedAt", "Email", "FullName", "HireDate", "IsActive", "IsDeleted", "PhoneNumber", "ProfilePhoto", "Specialization", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "HLV Yoga với 8 năm kinh nghiệm, chứng chỉ quốc tế RYT-500", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "trainer1@gym.com", "Trần Thị Hương", new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "0903456789", null, "Yoga, Pilates, Meditation", null, new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Võ sư Boxing chuyên nghiệp, từng thi đấu SEA Games", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "trainer2@gym.com", "Lê Văn Nam", new DateTime(2021, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "0904567890", null, "Boxing, Muay Thai, CrossFit", null, new Guid("44444444-4444-4444-4444-444444444444") }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "ClassName", "CreatedAt", "CurrentEnrollment", "Description", "EndTime", "IsActive", "IsDeleted", "MaxCapacity", "ScheduleDay", "StartTime", "TrainerId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("50505050-5050-5050-5050-505050505050"), "Yoga Buổi Sáng", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, "Yoga cơ bản cho người mới bắt đầu", new TimeSpan(0, 7, 30, 0, 0), true, false, 20, "Monday", new TimeSpan(0, 6, 0, 0, 0), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null },
                    { new Guid("60606060-6060-6060-6060-606060606060"), "Boxing Tối", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, "Boxing căn bản và nâng cao", new TimeSpan(0, 19, 30, 0, 0), true, false, 15, "Wednesday", new TimeSpan(0, 18, 0, 0, 0), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null },
                    { new Guid("70707070-7070-7070-7070-707070707070"), "Pilates Chiều", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, "Pilates cho sức khỏe và vóc dáng", new TimeSpan(0, 17, 0, 0, 0), true, false, 12, "Friday", new TimeSpan(0, 16, 0, 0, 0), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null }
                });

            migrationBuilder.InsertData(
                table: "MemberSubscriptions",
                columns: new[] { "Id", "CreatedAt", "EndDate", "IsDeleted", "MemberId", "PackageId", "RemainingSessions", "StartDate", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("80808080-8080-8080-8080-808080808080"), new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("20202020-2020-2020-2020-202020202020"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null },
                    { new Guid("90909090-9090-9090-9090-909090909090"), new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("30303030-3030-3030-3030-303030303030"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), 10, new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "CreatedAt", "IsDeleted", "MemberSubscriptionId", "Method", "Note", "PaymentDate", "Status", "TransactionId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), 750000m, new DateTime(2024, 2, 1, 10, 30, 0, 0, DateTimeKind.Utc), false, new Guid("80808080-8080-8080-8080-808080808080"), 3, "Thanh toán gói Premium 1 tháng", new DateTime(2024, 2, 1, 10, 30, 0, 0, DateTimeKind.Utc), 2, "TXN20240201001", null },
                    { new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), 450000m, new DateTime(2024, 2, 5, 14, 15, 0, 0, DateTimeKind.Utc), false, new Guid("90909090-9090-9090-9090-909090909090"), 1, "Thanh toán gói 10 buổi tập", new DateTime(2024, 2, 5, 14, 15, 0, 0, DateTimeKind.Utc), 2, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_CheckInTime",
                table: "CheckIns",
                column: "CheckInTime");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_MemberId",
                table: "CheckIns",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_MemberId_CheckInTime",
                table: "CheckIns",
                columns: new[] { "MemberId", "CheckInTime" });

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_SubscriptionId",
                table: "CheckIns",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnrollments_ClassId",
                table: "ClassEnrollments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnrollments_ClassId_MemberId",
                table: "ClassEnrollments",
                columns: new[] { "ClassId", "MemberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnrollments_EnrolledDate",
                table: "ClassEnrollments",
                column: "EnrolledDate");

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnrollments_MemberId",
                table: "ClassEnrollments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ScheduleDay",
                table: "Classes",
                column: "ScheduleDay",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ScheduleDay_StartTime",
                table: "Classes",
                columns: new[] { "ScheduleDay", "StartTime" },
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TrainerId",
                table: "Classes",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Email",
                table: "Members",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Members_MemberCode",
                table: "Members",
                column: "MemberCode",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Members_PhoneNumber",
                table: "Members",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Status",
                table: "Members",
                column: "Status",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                table: "Members",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPackages_IsActive",
                table: "MembershipPackages",
                column: "IsActive",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_EndDate",
                table: "MemberSubscriptions",
                column: "EndDate",
                filter: "[Status] = 2");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_MemberId",
                table: "MemberSubscriptions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_MemberId_Status",
                table: "MemberSubscriptions",
                columns: new[] { "MemberId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_PackageId",
                table: "MemberSubscriptions",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_Status",
                table: "MemberSubscriptions",
                column: "Status",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MemberSubscriptionId",
                table: "Payments",
                column: "MemberSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentDate",
                table: "Payments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status",
                table: "Payments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionId",
                table: "Payments",
                column: "TransactionId",
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Email",
                table: "Trainers",
                column: "Email",
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_IsActive",
                table: "Trainers",
                column: "IsActive",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_UserId",
                table: "Trainers",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckIns");

            migrationBuilder.DropTable(
                name: "ClassEnrollments");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "MemberSubscriptions");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "MembershipPackages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
