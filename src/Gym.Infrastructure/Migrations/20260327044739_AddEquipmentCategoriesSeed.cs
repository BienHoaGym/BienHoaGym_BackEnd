using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipmentCategoriesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"),
                columns: new[] { "Code", "Description", "Group" },
                values: new object[] { "CARDIO", "Máy chạy bộ, xe đạp, trượt tuyết", "Máy móc" });

            migrationBuilder.InsertData(
                table: "EquipmentCategories",
                columns: new[] { "Id", "AvgMaintenanceCost", "Code", "CreatedAt", "Description", "Group", "IsDeleted", "Name", "StandardWarrantyMonths", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), 150000m, "STRENGTH", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Máy tập cơ ngực, xô, vai, chân", "Máy móc", false, "Máy Tập Khối (Strength)", null, null },
                    { new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), 50000m, "FREEWEIGHT", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tạ tay, tạ đòn, bóng tập", "Dụng cụ", false, "Tạ & Phụ kiện", null, null },
                    { new Guid("f4f4f4f4-f4f4-f4f4-f4f4-f4f4f4f4f4f4"), 300000m, "ELECTRONIC", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Màn hình, hệ thống âm thanh, cổng từ", "Nội thất", false, "Thiết bị Điện tử", null, null }
                });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 47, 35, 513, DateTimeKind.Utc).AddTicks(8008));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 47, 35, 513, DateTimeKind.Utc).AddTicks(8667));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 47, 35, 513, DateTimeKind.Utc).AddTicks(8693));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 47, 35, 513, DateTimeKind.Utc).AddTicks(8695));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 47, 35, 513, DateTimeKind.Utc).AddTicks(8696));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 47, 35, 513, DateTimeKind.Utc).AddTicks(8698));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 47, 35, 513, DateTimeKind.Utc).AddTicks(8699));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 47, 35, 513, DateTimeKind.Utc).AddTicks(8699));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 47, 35, 513, DateTimeKind.Utc).AddTicks(8697));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 47, 35, 513, DateTimeKind.Utc).AddTicks(8696));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ8r0GvfmgqFzA3bIDvTkL1yHV8eG+wQszF2DLS5T+XDqaxI66vZK/Cem1qrHlh02Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMRNacf9TFUjhEm5h5HlxZOFZcTMKHXxv7phkKqtB9NGGGrTmLzLOTkxu0I25Y10fg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDLH6iV53SJJZMsrvorjw6NX6QUSYJXfe1uslxiKTVwPbR9E14U116PKTBjzsNYTEQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAeeaVEkuyNLk87C4TEPpR21JmXx25af2rgS8L4xczqvItRw0AzfrhIOfzfjPkx5bg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDYaAudzsd/89jZQAGXbAF6ESPeqLql05+XRVTotq3s2Jr8RuqgFu/yOt7ub0+yxPQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPiSz6D3ixTzzbSjPyQR9p/r73HbBxdtSehoUZH6398vk1L2HoU500K+Q0h8QUg9Og==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEK3M3lIZKdCbsx5gkzcIbWUtNnNTkyJcuv8dNTHTqRp8e1INWH+in1j3tz8S82AjyQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDetCL2jzJERjmO8cofiLvJ5z7MIjOCkckNYTE0W/cLiRChbD1KslqNj/SSuOp889Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBPLobwo1wCQj0qloRqsYDScA5WA2EskV3M0HB2DxqbK3iILmqsifaXr6jZ9+IzWrg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"));

            migrationBuilder.DeleteData(
                table: "EquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"));

            migrationBuilder.DeleteData(
                table: "EquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("f4f4f4f4-f4f4-f4f4-f4f4-f4f4f4f4f4f4"));

            migrationBuilder.UpdateData(
                table: "EquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"),
                columns: new[] { "Code", "Description", "Group" },
                values: new object[] { "", "Các loại máy chạy bộ, xe đạp, trượt tuyết", null });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 17, 29, 562, DateTimeKind.Utc).AddTicks(534));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 17, 29, 562, DateTimeKind.Utc).AddTicks(1369));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 17, 29, 562, DateTimeKind.Utc).AddTicks(1370));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 17, 29, 562, DateTimeKind.Utc).AddTicks(1371));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 17, 29, 562, DateTimeKind.Utc).AddTicks(1410));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 17, 29, 562, DateTimeKind.Utc).AddTicks(1434));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 17, 29, 562, DateTimeKind.Utc).AddTicks(1435));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 17, 29, 562, DateTimeKind.Utc).AddTicks(1437));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 17, 29, 562, DateTimeKind.Utc).AddTicks(1433));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 17, 29, 562, DateTimeKind.Utc).AddTicks(1412));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHxrag5FqenuMovsqtH7MK/18LngUK4VOM8GM1EBL0nQlKD3nkLoWNJAveZMNxHoMA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEP66Ej+1QXQ4Ql9bqC85ln6ortecpbwh/Is+gxnhJtYVIUkAnDY1BZR/telK1Zo9gA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEZnZcTOGWHiu4cANoWkxY6d2lCxLyW8TvXL8Mf7wWzriQ4whoZYemUZ3H2Kf3vO1w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHSGa19kj9yHhyuaD9kSnX9/rDA1k8Ny1ec/9Ot4CEIC0YHnlryAFcm3Kf2ok13ilA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFwhdQ8u1RmIXyP+sK9InzF2IotyUVB/LwZ7G6TSrpxGpIpvOYAp0g2wBPxqR2uXnQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIrHTMGy7xJibb45Uy3880KHSi1mrVReewVqhilfZa6Alaz5DaTtVXgb2nEYXX9lWQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENFb/g045jqy2wZvMMRPaydoGkzvCiVlnLJzex4mpumS86VzP60Lil+L5uv83nesNw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELtyR/XuoraShmKL1NUD07Q3dtGxs0V2QnT4eMIDKNr1D5JxuXuAvYOjHeX1dBngfA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJBObG9OTt7LlylLCRCqcjJBwmey3P/0+3i0eex4tWSRpcAT3xRPxkWiRRSEGfJKxA==");
        }
    }
}
