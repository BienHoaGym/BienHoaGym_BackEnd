using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 46, 59, 535, DateTimeKind.Utc).AddTicks(4630));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 46, 59, 535, DateTimeKind.Utc).AddTicks(5879));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 46, 59, 535, DateTimeKind.Utc).AddTicks(5881));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 46, 59, 535, DateTimeKind.Utc).AddTicks(5883));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 46, 59, 535, DateTimeKind.Utc).AddTicks(5885));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 46, 59, 535, DateTimeKind.Utc).AddTicks(5902));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 46, 59, 535, DateTimeKind.Utc).AddTicks(5903));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 46, 59, 535, DateTimeKind.Utc).AddTicks(5905));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 46, 59, 535, DateTimeKind.Utc).AddTicks(5900));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 46, 59, 535, DateTimeKind.Utc).AddTicks(5887));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJYQVvAk5hSxQO2/RXC7hdUYAKIdRWzZnhYoyljHmJ87o6XlWDASBIPnG+8e/oBo5Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJwTsSQW5H+tNS/9gs5Ay5VeQiX/GITboTWNukqqJrhjvc5VqpMHn2XimTJfUMN/DA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEP0kDnz6bYM//oD9A0njMLV6PVDti3U1zKpjL61/GNL/prcutuIYv/lGNx+84CK57A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGCN2UQXW5KyxhhVrZbmUTdul1YPiDvA2Bd80ayZhWmcm2lSyBnc8wE0Z2x92ZM/NQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJuXDXa4wvqT1qc0MpDwRb0t4tKaBHF2HehjOSP/HN2DLZsilZ+Y1jfbDytQbOzKLw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJsUHLCdW1D8mqhDLczGXRmTmv77KcesLEopf6llKzbYShrbhWBkTCF3GfYylFj0bQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG6U0yemIvxOcTRON6V+vsuzXKV0t/T60hna2Pm77T6tonWvCZgr4V6+5vnnjgkGMA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHtafHYu38/8vE+rMSU/lf9bCpghKcYjmMwFC/RagvVp9NZpoBFfg1w9dUs1Eauueg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPZf226BFs7zDOGaY3k4k4ZlEhYdYsq6xL3KLGQSJETIkJMJZmZhUg19gLmQvDvF3w==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(2727));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3603));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3605));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3606));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3607));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3616));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3618));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3619));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3615));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3608));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF1gjyXoif09w84iyQRROxsukYCveXpFAB0S99sp3/lhqJZRJqT7Qj3tOLrAEju6zw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEhbdHKBThGiRqpgRyiRQF/I/fvDQjzUaxXTohxbvwqfdpWMvx7NR38RAafME8oEjw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKBf1Ntua4GObrmTPfnSrnHooaxqAKXFBGEbOeOipVkHE96t1GqSNQTflucObYdP2g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELnntOhyhrb3mrcTOk00bwkLJdI6uV0by8DK04GtsH2lQXGN6Hd8+MfKtYJTOj6FVw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKUv51+RWnCNd3Jbx6yMEo8r7AWUYMKvNj1XdWapP09/ktrtBkx8dq0aL2x8FKlrRA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBrrHXAvj8gAPuWggULBUnGa4zG5U6oCto5BlZBsvjjlZblIPwD7oLfqWOpo8pIHvQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENOzXUl7ARpVabc9r7n0y2qEqgIf2B1FvXIR8RIA/ZxdLx7BiFrZ1WbvBdIOZbTtKw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFnAHo8vTK+SJc/CStD1tsMCQgysKVVLKJB1Fk/Vi5aOc8/f/oZ2j71O6LseQq/KvQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBZo9rL3k2+/w/J8ekN/YA+LE/5SxFqxRNovl8qiW86KudW8jLyIg8zASuefTlyJpA==");
        }
    }
}
