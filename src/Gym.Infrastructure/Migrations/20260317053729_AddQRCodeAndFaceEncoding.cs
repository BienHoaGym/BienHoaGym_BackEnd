using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQRCodeAndFaceEncoding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaceEncoding",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("20202020-2020-2020-2020-202020202020"),
                columns: new[] { "FaceEncoding", "QRCode" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("30303030-3030-3030-3030-303030303030"),
                columns: new[] { "FaceEncoding", "QRCode" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("40404040-4040-4040-4040-404040404040"),
                columns: new[] { "FaceEncoding", "QRCode" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGdFRXUXw7sGpQQbxXqeqXmh3puYzvHHoUfh/hDEAseeLEksyzIq/DNQPHC9rFnnkw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEObcTew1pSkVfJw5uoCz6zChmMWWkxp51Mqu2x5s0KpATXIAa2Cyv0+twvI9+01Rxw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHFBRuzSw8BhoOKoodkM4VyHW22teKIMW9sgaScSyx1mP27CjzTTciGMG2nxbWdEkQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGNJuUlaEyhpzXWM7SVnbqL7JpsKAxDcX9og1SayKJ8fNKLgfeNuM80Nm7ndby6gSw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEARhx1EieDnVtW2Ug4npQza0y+5NdVr1ICRobpTjnj+jUnjLwm4tEr+R6OqPSCFk7g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHIuOwGsMgcdgxJhuRK5LFkHh1dqHTBEVqU+hS2KiYknkL2dGfSVlhGZ7CTaY1EMMw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDDTKLQYusgACAduUi/glR8SGEMa9qffplOUWCdRfKZjkY4X9CqyX/zTytf5+z6QOA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEILdk1ilHKIMR2ls44670Lzy7s61Op/CO+J0gbkyRRV699DtbIgFx85ppXl5G68KMQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceEncoding",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "Members");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG255+CppNyPNcsa71Bgaofynky8hpJCXi37U8LedPEXGmAeh1S4vHHN69PVeDjUCQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMH1W2Rt74WwXymVhKejFNH9MICQ6SfojhGaX+D0ESScaL224Zdf1YxDZLlb9SyK5Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIjr19wSVTWkxBL7e18fo5ZAI7Cnm96AvenPXZ4DPrtM04e3QzowMDFi5t62EWo3Jw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPYgJ6cDAqpZjL7smJk1B/+4mNEN5oNWOkmlU4nAmx3y6YKdZJFZToSaLxFUA6Iuwg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGVTDwukShulrwuUriWIwzv83ZyopFBzpBiHFTNPbqZHhVgB8J2Lj5JMaM7lNS230g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIAbh+J0yqHiJfcSqWC24/QYyI1P2wtF1CmSAh0TwYgAFX4UFukco+2+5u/o6LvDKg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBAoC3XAexwrOuTaiXo7h73esd6RBTuxfsBMs+5k/kEt9PJM/gTFIkkj+uENPIO+ew==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFcJqcoNkeo/FCaAdgAlJ75sGpV5rgKhQ14q1JxGuU3yHhz8f17114HyQrpi5scPDw==");
        }
    }
}
