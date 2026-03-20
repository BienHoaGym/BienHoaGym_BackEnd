using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class member : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyPhone",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("20202020-2020-2020-2020-202020202020"),
                columns: new[] { "Address", "EmergencyContact", "EmergencyPhone", "Gender", "Note" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("30303030-3030-3030-3030-303030303030"),
                columns: new[] { "Address", "EmergencyContact", "EmergencyPhone", "Gender", "Note" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("40404040-4040-4040-4040-404040404040"),
                columns: new[] { "Address", "EmergencyContact", "EmergencyPhone", "Gender", "Note" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJm7LTIuTQub2VXLFNdtHJJihD3veTvwDnbTTIPbRZjrncPh4ouO5QCztALsSW+Oyw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHmEYV9prAwMIoxCNDRxcZfKaKudjbd1SSM+stIzPFi1vidxWAgGUNDiMgJUErPahQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECx3Id27Kbhu6HGYtiboKpvKgOnRHt/9EHMS68i3v087TSi0HayRCsreZciwmX1/+w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ3ozhXQC223BTfZQ2pOP4nFO7aRorFvoob0tx1/n9FB+0H9txeA+8Z3EsUeVmTxnA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOGpFvUTf5q+kziDKJdKK+DZpeae5+QKDZdMCnYE0jDV2frnioLKb1zMfYn4YJsdHw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENpgA01P/ngMPjn43mJhYCdPgrBA3tW4Cr0p5jq6Vldy9Ue6UWoR08m6JeKcV8MIKw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFUmbY7DFfa6rtjhecz+ACYUYmE/Xlbn88hNz3d9re0/OAkUUr/BG9Sgiqh9vM8mTQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOME/9hJ3tG4mW2937bQytVErggV+rZkuSHgQ8vtxxbyjchWOoulvj6wMXjpJyzqtw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "EmergencyContact",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "EmergencyPhone",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Members");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO8xR8H3kdW1/Ms+5ICvP5AecAw3pb/NqfInydOWhiVOKFjhwxEAyHu7OcR3BAIHaw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELYZQys+6+krI+QG6i3r1rb2fTBDROyr1G6r7SYD6LQWNazY+aKzUoVPwZfo/rs6bQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDNLNb28l6ynkF45XkYtNbr1fh1errBuDHwuxC7wwv1fVHsWr1b2c5PZ6zmscCEbew==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFozpoOQvoviFF9gqN2DXMKoEa+a1DCQI6ose1asu5CC0GSmok9qK7alDyfRZYpNpg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHpoIDraGukJs9nlMtrhHBfXwBWB1mhyFpUj+PSXfCxiev+JgnW39jrZx/5rtXRxCA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED/tNn/fm5QZJ+VuiRtQolgtDFtTJF2DCMzBGxuQawFnK3AGDo5rma7gbmW3GlOlAg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL/GtPVvlgDJJsWFY3ApTxLbrXRB9dRiq5eJskbavYHeKHzRbGxLxE0wEuU8PbmzDQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHdmlFshwh8hm2ATzWXanfeWeyNOck/UPiWDwq3qGmIyiQUcS8fRxFdVHLH+vLnN8Q==");
        }
    }
}
