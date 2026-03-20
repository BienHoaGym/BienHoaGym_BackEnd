using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceInventoryEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinStockThreshold",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledDate",
                table: "MaintenanceLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MaintenanceLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Technician",
                table: "MaintenanceLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromLocation",
                table: "EquipmentTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToLocation",
                table: "EquipmentTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMaintenanceDate",
                table: "Equipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaintenanceIntervalDays",
                table: "Equipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextMaintenanceDate",
                table: "Equipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Equipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFCqnLKXASsTCkEyIfs7IBNeqDUSx2Umg9sJPPrnU3mYegH57Ps80XPWRbszW4Hx6A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ8jXhr4evdrxCDqR8+QEE6BUWUQ3k2C7wYoY1ytiU+kjpXl948skzG/jzk0QKENmQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO9HMw6LGVLbL2yy20u7VUYupCBsJFsuZ3mgL9FH2RN5iVnY+HU0y9Xn7qfWUguhRQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMd6McneFwFq28o8r4enQpiF/63/YfJMg1Tha4dw4KzeWF/HVPK22XIkdE4Unf0q/Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOr+EnQTI2YzsXqZA8mANjUL134OyFMELDp2eyLdjOqAoejaMaDCH6iRTLImoboEHQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF+2/5kB+aXFueaaR/pzC1fqytziYoqL8n8/9bn55kRHNLBc0gr9zmVzv8dNz7Ra5g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL3tXlghERJtqvxTDdK/F3Kt2KHrc9/IP2eVFzYqvv7YZNXD3/iHmfeWPjoQmHNw1Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMV5nUGpEUs68Q4DTBljnAiNDZshW6uAjNaF6vGqY8FPJHYZSH6M+mufhfFame/uTw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinStockThreshold",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ScheduledDate",
                table: "MaintenanceLogs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MaintenanceLogs");

            migrationBuilder.DropColumn(
                name: "Technician",
                table: "MaintenanceLogs");

            migrationBuilder.DropColumn(
                name: "FromLocation",
                table: "EquipmentTransactions");

            migrationBuilder.DropColumn(
                name: "ToLocation",
                table: "EquipmentTransactions");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "LastMaintenanceDate",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "MaintenanceIntervalDays",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "NextMaintenanceDate",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Equipments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIDOXjie0a27SJBkvKaDMkI306mOado58dsmoT6/K4IrMYC8h0TsaQMmpwskIWnq3w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFXgQetnzfW+PScMXQtu6GzE8bf583Xels/cIKj6mRWe4JYcQ/zHIBBCJjpOt70enw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDcI3jYdLY6eRrxc0tmz3ixeIQ4m8h/PWPAthn+jhNrmdcoBxcEQ0UVpOv5rviBFyw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPXVASXRRccJVhNBIcB/1Qz5+EuY0VypGxRj1VRYYnaZMZ5CE5Hm3SuIakamWDUzoA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENGbSTIzwKDWKSBGcjmbnYdOC9Ocx0jqvJri58XGw72KfIEiNODbS+7h65zgXWNs1w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKZz+K3OtJhYMS+b+EXk2czZWujPfxm0unr+cHgf+zLFpq9oI1qeHNIa+FzGzpd/fg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA7WnR2VWtglpeckTH2DNdc0e/hx7msNKJIm+1j9xN7CvVJ8ETe+u44rIciwTLDLEg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKHKrmYcJZXDwIfe6BiaDNFzlXGsTtNFke3RiA6cBWxmsUmyTStu6LSLXKgBvADrhw==");
        }
    }
}
