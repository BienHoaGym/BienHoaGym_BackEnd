using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExperienceYearsToTrainer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExperienceYears",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "ExperienceYears",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "ExperienceYears",
                value: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperienceYears",
                table: "Trainers");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN7PvDguZSdhMsvGIef9gs9HnDIOFfCIFFtDLAn/yr41aW9HTrKygO68XXiTEz+E/w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOl4F+bwXerCsDqS7o3OiBtrvXtWPgJSy3KjDwaA/YKWxkLaLqUit17wFW/ousyPFQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKgbSQqwpUoQUY7988IxdjIh0xL3yuLTWoxhgSozG/tJwaqNLAag81xeC0rjMdZISQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFxwFlXohf3I6KWOlto2zdTCBx9vdcnpub0r13+jiC0VGU1ueNwxfX3kINMwFXv3hw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJbrNnkXe+bL2M6Lv/DF6q+75RgNI5jJotOHLifarZ4M4nssJ1IqvNBqlIhvcMRoMA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJy/iXk5U6a7CZNNUyseIP2jJbYYH2gFMBnGYxVlLyP0QXc4enfFqaiVpfact3fG7w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECMllJrIVSR46dmRKcx7Gx7oZfIHZP3Iw1o1kfjvnDWjUZk6UWE/kiP5d/1wI14dQA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENQwwh313O0qBvodV9w0Qg6N0bMp7huYZgzlZI5irSSFIWoCSgCSGsP0B10KZuKjZg==");
        }
    }
}
