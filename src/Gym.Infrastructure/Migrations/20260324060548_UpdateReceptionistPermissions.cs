using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReceptionistPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "Permissions",
                value: "[\"member.read\",\"member.create\",\"member.update\",\"checkin.create\",\"checkin.read\",\"package.read\",\"inventory.read\",\"inventory.consume\",\"report.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"payment.read\",\"payment.create\",\"billing.read\",\"billing.create\"]");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Permissions", "RoleName" },
                values: new object[] { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "💰 Accountant - Quản lý tài chính", "[\"billing.read\",\"billing.create\",\"payment.read\",\"payment.create\",\"report.read\",\"package.read\",\"member.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"checkin.read\"]", "Accountant" });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 5, 45, 100, DateTimeKind.Utc).AddTicks(3144));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 5, 45, 100, DateTimeKind.Utc).AddTicks(5108));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 5, 45, 100, DateTimeKind.Utc).AddTicks(5112));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 5, 45, 100, DateTimeKind.Utc).AddTicks(5113));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 5, 45, 100, DateTimeKind.Utc).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 5, 45, 100, DateTimeKind.Utc).AddTicks(5130));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 5, 45, 100, DateTimeKind.Utc).AddTicks(5131));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 5, 45, 100, DateTimeKind.Utc).AddTicks(5133));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 5, 45, 100, DateTimeKind.Utc).AddTicks(5129));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 5, 45, 100, DateTimeKind.Utc).AddTicks(5116));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMbQ2q7sEU6bg4b9cdkQaIy5LGjYdN9Y+FOBwY2DG+TQD3sAE75/cw8HHlJX79o5fA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFVlmLzl0+GbRO7ARkoNXQE0zsk8/bEm8QfGvY3JW/hL5kcSHrCq0sveVhhKrXhioQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMTdlLLHF8YVkfrho9E/TglA8QrNOx+a+DqhpoprQMyNOxPLX9b/ae17QPoI+d0pNQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPpGCQLNGC4hdowODbNUmwlYOtXv9AzVPfPGGZeXmGf/8INmSnpLzs3JD4hlHTE0tA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOCVf+jT7cxcYbjyPnxo4g3xitNncX2pbwHDNQ/bKpEpFj/apsYiclFClrcyvBhPrA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFYmtTsSIu75y4f0NcA2+RdyXQDzzYagsn7na1x+LdDpATJ2xyoIsSwXAJfUYTR0kA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFWnWsN+I7v6RHUDILfAQbMu9xoK5aGzkUZ9D52cE+nFlBJzWyvvXpoCPQvtmEQGxw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPuFy0oR1176yxToOzn3CiWprs9wV6gwHMpcYkrQb8KMlePlWQr/GI/Eh9ehhp/9tQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFV9Xm5Bf+F2vhkA1FHZNT7ukjWA06/uQGGb9uw0vetUFXg5Uxk9cUjEVwiWMDWw+Q==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "Permissions",
                value: "[\"member.read\",\"member.create\",\"member.update\",\"checkin.create\",\"checkin.read\",\"package.read\",\"inventory.read\",\"inventory.consume\",\"report.read\"]");

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 13, 7, 36, 648, DateTimeKind.Utc).AddTicks(2133));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 13, 7, 36, 648, DateTimeKind.Utc).AddTicks(5336));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 13, 7, 36, 648, DateTimeKind.Utc).AddTicks(5429));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 13, 7, 36, 648, DateTimeKind.Utc).AddTicks(5432));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 13, 7, 36, 648, DateTimeKind.Utc).AddTicks(5434));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 13, 7, 36, 648, DateTimeKind.Utc).AddTicks(5463));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 13, 7, 36, 648, DateTimeKind.Utc).AddTicks(5465));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 13, 7, 36, 648, DateTimeKind.Utc).AddTicks(5468));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 13, 7, 36, 648, DateTimeKind.Utc).AddTicks(5460));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 13, 7, 36, 648, DateTimeKind.Utc).AddTicks(5436));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFLu2SFkjjrRihp1/8g9hS/tSl2QKtVYfIq3qC4VjqUXTNyT5d2Kncbb+vFGWcN/Mw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAi/gW75c7awHtWIKa4zhj1rmWXN9Uol1Vk3jCNCwyy7jwEzdDovD7yRP3Gunxa8lQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOSs0H1/G0ALgFSkxxt7yXds+7AyVNv4QqPbzvFZ7ZIcp6ddXmulk7f9gKqH4vRBXQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELfvtpFSHKXlKfcOspn703WN9EuhOsmExclkac1WgBCrbDajNLy0cK3q9fbdvTb6og==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN8ERx6KO9FdhtGIhC9oGzLvoI8+3AkThV98FQMrRKhIWo4NLSqghYcZYlis34PUXg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENy5y9RdVMLT6jy+SBYup8whl3QMOHlxCK3reUOREuPiEqjjSx0GjeP5xTEvpNjV6w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELYY1/ndDQE2kDG2QybyfmtfKNIC0M/tWuUXDWP7w43h6NIFc4zyo98BMNCyEfXlOQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHnZmIux1KrvLl1kBj4WU1L0PbrUCFBzHymhyKXgYCLa33pGhDs/RR2/6XmDXeSClg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPa+BeUS7avBbcfPaIa9wNDW/wDb1DnTmUwJnv42MlE555fGmFrLqOh2alWh2CGUtg==");
        }
    }
}
