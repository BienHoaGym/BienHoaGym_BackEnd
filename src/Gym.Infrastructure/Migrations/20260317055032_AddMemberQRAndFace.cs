using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberQRAndFace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC4k3cBBY2VC0mQ9fQTlcvYV4aD8qX+jdAlKP/iUHVaa6EE2yOktQMSoTCPk0n75ng==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEWEStJAdF7uADKC0+rC2nge51sSbk+pek2+xCqvgzGQaBWxQFuAmKtlmOvc3zIIzw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJpBmqtoIlqpRcnlpQq3lk90tizOa+hEVj3ILENKF2+q6OaUsIQ/QmWC4BXStp76QQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDtCoN8pXgvooZmY8jWlMrlO5I8exB5S+YgT5hZlhWricYm16TAwOITouwJhXDO/9A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFFGNGfl8J6DXewbiWrzqqV5eXvqKnFQyVyEsb0Z0UKeo9fRVEFN+tXUI3urPoZj0g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBrdEh7vVQFao7SHkCxH84T/ZYZLGF/usYTcUK+l2JOxkQRU8C5wmh+H5YaOcGADaw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE/+QZO2oizJ0zwA/OkgMZyaN5e0Qkkuv/zzvm28lAYZrC8gE2DHE8u2RNYsgHJ0ew==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEK5qBbSZWI7an+GJxmZyHa9TQZfoNu91MbeVLFDzQVr+AG8a5sP5mwVsQVwt/wDzMw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
