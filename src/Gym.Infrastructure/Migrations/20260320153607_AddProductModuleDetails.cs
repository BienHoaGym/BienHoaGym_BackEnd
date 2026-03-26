using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductModuleDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "Products",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DurationDays",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SessionCount",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TrackInventory",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL2wvjjkHqax12R0IeO+ODguHwmli6yrLHVA8J5m0CTDh62GaVnX4AQ+x6vZkHoIKQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDHh7OV6lje7fBgaONQUYkXCpquRud6tPk5tWUC5M5ECtPS5lonuv68WBkVpp09ZaQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM9tZAp/3jtyHi4NU2+/KgMF/58Lh7T9ouqV5OTPzcW08vJMcHzeBlbpMjiSQD6nYw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE40F2WdhaXvs8Ety3v5yZT9TFxlTki2Jj5w3TRjGDtUbXG48GCyYk7sTYVOo4YajA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECAqoY8uA+czJoMVALmMSoK0+qgOnK0kybFPALlaKbe3UfiyvA/YKAnwnvJzCmfq9w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPuOhyq8sS7QOf5JWNv81Hu/vUF5zj2EiZjp4DediwXtI5B0dJ4/R7xMlg3iXdmFjw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEELveAsQ3SY71Q/n1j8zsJsecQmn2VKh1VXCwfgoS6B47dZm1neH7jLEztv9lJfX+w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHI50zn4zIL6NRibpYN/CuMZbWkiER7zgY7M0Mk6y2Y1t0qY1xKBT75rDFw3zlsvoQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DurationDays",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SessionCount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TrackInventory",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMqVKfIIf8gj/tlT/ppEdSFBoJTTfnIvvI+UZoR2ZjrFvxEflXPTyt4kIvrsDlPtOw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENzQgb6OjyS639htIHmW+0qjQd6ly2QmxyQOfhwai577vHOjlaj9NBKzzbomVb+CWw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG7K0fkGOV/Bsq6fa4Wrx5iiqyzKSYPgDbiVg5raXpBG1OuT9e9DyNijWBkp5BB1+w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGIk+b+RacP4u2TNxWKp2rDakmJB31ugFYd8FNOd84Usr/bvqf7DWp2CTa8aMHF5gQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECHnAnj0fk4cyv9SDSSe3smrqcMwdFVdWxNz6MJdPDVrcOvLjBIt+QBZnJqq+8ZZYg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGM+5zLo9mxTOyS0MdmDg03YngaYCbZB84dYgwUrb/PBHrnSFosmIjaw/4j+ZgrqMg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHp2LMdOOoaGQbmKjBpYvtSc9NiCPwMx6IvPhpz7YQpQTDgbm7lbsKvuhIE4ys1ITQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG1R3HsRY4kvwk/YGSgvo3Unmo/Yv+Ifx7wf77sjgOzCUEww1yobXnUPJWRd1Obvww==");
        }
    }
}
