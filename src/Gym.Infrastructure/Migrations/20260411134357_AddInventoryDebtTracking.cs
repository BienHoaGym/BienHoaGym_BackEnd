using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryDebtTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentUrl",
                table: "StockTransactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "StockTransactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "StockTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDueDate",
                table: "StockTransactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "StockTransactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "StockTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatPercentage",
                table: "StockTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDebt",
                table: "Providers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "EquipmentTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "EquipmentTransactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "EquipmentTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Providers",
                keyColumn: "Id",
                keyValue: new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"),
                column: "TotalDebt",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 13, 43, 54, 597, DateTimeKind.Utc).AddTicks(5323));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 13, 43, 54, 597, DateTimeKind.Utc).AddTicks(5339));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 13, 43, 54, 597, DateTimeKind.Utc).AddTicks(5342));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 13, 43, 54, 597, DateTimeKind.Utc).AddTicks(5344));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 13, 43, 54, 597, DateTimeKind.Utc).AddTicks(5346));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 13, 43, 54, 597, DateTimeKind.Utc).AddTicks(5352));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 13, 43, 54, 597, DateTimeKind.Utc).AddTicks(5355));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 13, 43, 54, 597, DateTimeKind.Utc).AddTicks(5358));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 13, 43, 54, 597, DateTimeKind.Utc).AddTicks(5350));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 13, 43, 54, 597, DateTimeKind.Utc).AddTicks(5348));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOViDr/azOPg4rxw5BaW9rmP6dxr1jkKE55rHVDyQqx786yfoukNxwG9hAIBP7oxRw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAUtvOYHqfmm0Fxtd3/9lAgocwDuIHEhjE8QqiNn89u5wIiidim3iyAU+hvF85cO+Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBMvDykF/7hYQ9UQcnGlmYYUucHswL5YZP00qQQ7ac5Ldcw1EtledelGhAJwnT95HA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED/tnLuglYop9AbRZ81f3QynaW0PLFQy9FLG6bZAJQ+TewvJkw+AggXctIJnvZ0bYg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFWNexI0jf4lm+Jihwmnygh51b9IgLT7sBvesYatHIhWHhbFcpfabDB+x853UKsoxw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECvAnli40qTWwgatrg/LVNbXVwvhOpsSFh6hS/Jl9GPICUDgbwExrqAgQ0J8dqE15A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPGFQOLU1b6Bg/sLRGJ60CWXRZ4jiSkvYupDDb1eKsezntyL3yNRA5i/ALoXZk4VmQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOYpwsgBNr9431FDuofVyW63rE5KbK4snqWI1Z4GUOxBE80QlMrI1xDHieUQM8slBw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKdr/2/HgBLWCAUwHvgzT3obqd/RQuIKY4xOw6ZEUY6s49WdqDm128xuWanGZOzG+w==");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTransactions_ProviderId",
                table: "EquipmentTransactions",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentTransactions_Providers_ProviderId",
                table: "EquipmentTransactions",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentTransactions_Providers_ProviderId",
                table: "EquipmentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentTransactions_ProviderId",
                table: "EquipmentTransactions");

            migrationBuilder.DropColumn(
                name: "AttachmentUrl",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "PaymentDueDate",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "VatPercentage",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "TotalDebt",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "EquipmentTransactions");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "EquipmentTransactions");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "EquipmentTransactions");

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 52, 2, 298, DateTimeKind.Utc).AddTicks(6868));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 52, 2, 298, DateTimeKind.Utc).AddTicks(6875));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 52, 2, 298, DateTimeKind.Utc).AddTicks(6876));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 52, 2, 298, DateTimeKind.Utc).AddTicks(6877));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 52, 2, 298, DateTimeKind.Utc).AddTicks(6878));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 52, 2, 298, DateTimeKind.Utc).AddTicks(6882));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 52, 2, 298, DateTimeKind.Utc).AddTicks(6883));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 52, 2, 298, DateTimeKind.Utc).AddTicks(6884));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 52, 2, 298, DateTimeKind.Utc).AddTicks(6881));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 52, 2, 298, DateTimeKind.Utc).AddTicks(6880));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJiP9wDWDYFT5xTbB58LKDBO3y6dhaVZTXWIIUYKY79YbGZLGkRTtEHEPn6VICdjpA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELItfXrSvdWC48Ud9pWxIzxOv0QJPK0ZkTVX9IdtAu/f4DUGgJvLbLKR/yeDwaPYIQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL4OJzvuhKZyd78bcMzLKAflkGKMerUuL3rJPQLGamZTAELuC+xUA6COWHOZgNDV3Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM/3K2SRU1m14mfOqpY77y7hCVXO6B6G6JymWZSn5GXC8B4dN87VzQvvd8Y1lNJUJg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEIqRgai+fht+khSzQHlhziXIBQUdHb4WHQJG6+TsDVMFwrV4kRlADhm3iY6lF7yXA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIb5pSL2YyWm9OqPG6Az2VBSsOQ1oWdy+nf/2WlRLpAR144qRZ3vCJVOz9RaR/yJmQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN0Weq2Z/G/cjMWS9mJkICNCKl3g3gMH4zhwUiPCkfb780jR1KqVlVqyXyeHg0P1yg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHOp0AFbYqok2qX6lm+WaYppWwmjonx8tx9ohHywV4WgwD14q8oFL7pIotC8JoAHzg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECNV/F5rrK/GE5EZ/CFr8U5onGyuhruxxr8YEZWEagA3OqvVyEL2t3vINJznHU5Eog==");
        }
    }
}
