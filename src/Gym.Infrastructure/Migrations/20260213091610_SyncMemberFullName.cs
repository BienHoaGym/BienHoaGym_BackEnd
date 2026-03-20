using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncMemberFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuccessful",
                table: "CheckIns");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "CheckIns");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                table: "CheckIns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "CheckIns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEClqrnMBMDIroIS5hHY5GV4up557lrhItzjPTWR3ZpZWzJ5NT0e9xWwNZf1jtIs84A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIQ4j7xg1hfhTJY92JWRRpzOXSkM5QIkfi8ui52iSzDXFibAfIK/7NgIVqHBR+OkBQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHL/LGKN4L6LsuomdoOVR2UWlR9cEECM/1U0r1IaspG6Ms00PFkRD0xSp/51FiP/Aw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENpNYPFdYJabPl4RPXmLXdJlvJkl6cv3J0Tesz6YSD0fDaJTfsuFsZkaTj+PD6yWBA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFSdS44jklNxxLm8n23HVfRqn8eIU66xV2RR59HqXry7Bbshx2Fv9oOVLEmZzeO32A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELNLn9B2igkqz+ouDxmqg4UWSfFcxnQ9wx6aWnuexeIfHkbdEOog2FTDFo4okhEy2Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENXe+6kAiBCYp2yk3fT+w9mUQX1H5EJp4XIQsQAi+d8pO2AK0TBukm95d8ahQ3Dd9g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF1cW6wfYAMGfqxmTqELLMc9wXuG86q7RB/nutvdJPTLZKzsXffoNGAayQtYBUvrDA==");
        }
    }
}
