using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Members");

            migrationBuilder.AlterColumn<string>(
                name: "MemberCode",
                table: "Members",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Members",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("20202020-2020-2020-2020-202020202020"),
                column: "FullName",
                value: "Nguyễn Văn A");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("30303030-3030-3030-3030-303030303030"),
                column: "FullName",
                value: "Trần Thị B");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("40404040-4040-4040-4040-404040404040"),
                column: "FullName",
                value: "Lê Văn C");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEPB+kw0khm2+zmuaA93mqGST0+qTLg+gOcnlSQgwq07Buojsupi1b+4QxZAexxKUw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELicIUqGOBiqaeDUQH7bGd+JcrvJKhOTqv5v97no2sDcrMPmc5axoABhMYmQ6aOquA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOvEvcLetE/TBQSG0kQtU6vLiV+EuVlLMibT34rBu2jIBrVmO+GMGUeBSSWzeDcDFQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEETxaJ6ZVqjKKe18TQupFAh9HKuim3bQvttTKW8MPMrnhud7po3w4SgAjBLCtdfv4Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMfoXtRovFk0TnyAOy0pgthbLm0Atxa0DNq9VqemORHMJHaAtlqmlMXn1c0T1JYITw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEam9a1GTVnSL0S7Vx9P6aj1A1Ad26Bs4QsNGUhZtBXhk/QWo2MCHgsaM9ioc7kZQw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOr/PUW4vl2jUr2VAIVQU+xcEfeI1qKbKRMxaTU1b87hYc8xWwHmqRA1Li3FRh6pdA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ7PhSKu2GLrC7WeBupK+f4oUO968au14BYKWnYKfWPCLkKi3/4cAcUQys9X+mDnwA==");

            migrationBuilder.CreateIndex(
                name: "IX_Members_FullName",
                table: "Members",
                column: "FullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Members_FullName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Members");

            migrationBuilder.AlterColumn<string>(
                name: "MemberCode",
                table: "Members",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("20202020-2020-2020-2020-202020202020"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Văn A", "Nguyễn" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("30303030-3030-3030-3030-303030303030"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Thị B", "Trần" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("40404040-4040-4040-4040-404040404040"),
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Văn C", "Lê" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELmi40BRuMhtMrwiPb2SA3OK5LE8W5pxEYRlySb4aks3FnDT6pR6rLNOiD6WvkUApA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIL2saMjGHgKBKol0a2zmc9GK0IM15ahgzTRiWTi9j0m+FHwIaDwNfDS1dzcqe9yWg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEB3kLTfyzlpcm/I7r2QNs1O763HgqKAH2TB43qK9VIB/bETroc5L3ziGNQ8FCR8FA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEACe9K5dvMkXvSmlGM0cYOkU1VNyAUkZ95f8oexvVZegfmx5ASHpSTPW1MAbxPWbVQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFR0hIlN5AuK0ml6ZD5wgK2Ub/VWCQ4uW1P5pQvfSQ1no5Mj2unAO1GmUu8/gk1Bzw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC1h24VJSVlpd6hsbzSmfeg/og6e3TBjIpQef5bmHc26y1GocmaKCHB9NqyCD26Zrw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKpaBNpBoI8ATRBiv5Oi3YGhs0MQy17Itu6Bw+i6Iuhvh7s/N4FuXJye74V5PvSdlg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEK1pYiD+xCDi/Ek+C6JEP7J92qyS5Gkjt6Y6GWPDA176mWiGiMdW/0sKAdqXM+jsag==");
        }
    }
}
