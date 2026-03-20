using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Application : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OldValue",
                table: "AuditLogs",
                newName: "OldValues");

            migrationBuilder.RenameColumn(
                name: "NewValue",
                table: "AuditLogs",
                newName: "NewValues");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AuditLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AuditLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AuditLogs",
                type: "datetime2",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AuditLogs");

            migrationBuilder.RenameColumn(
                name: "OldValues",
                table: "AuditLogs",
                newName: "OldValue");

            migrationBuilder.RenameColumn(
                name: "NewValues",
                table: "AuditLogs",
                newName: "NewValue");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDq3Zs5DhUYqr//ri7yN9oRe81duK2bv4eQTjlXUwriVn1lLewQO5cvzkR8qjPrzWQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFejpPhGJMaTf7QUI1t23ZETsTBePJ8Cs09dd6c4zIzK/IM9odXzcTJOnPZEJrLhoA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHrzR8Gz/P4KC5lwtwuFcGDXVxV5LDJC+90rQDGXwpAkGD3yJfy5p9SiEICsN2faOQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJrToWRDxxQrijgnkKFInx3YnHc/xQCOn9TJgDZGos0Ss9RExVMAXSIkJ9e1jDVZeA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE0d5dLPoEM719xRaj86K3i4hJB48n+6FjjN9JrarTrbzOWLrmV+9Vmjv2/YphV6Sg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG2+jfMJRJrzcpHl3WMsDq8tVtjiYcvNTHP3bl2hZ5ntkkqfUFAOS5nkFscfvOPh4Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN1Sz7K6Tgvw8bJcUk2GEzgm8EFDEuFya9DB7rlH6swSFOgV9jH8Tf6jUBhAbmSVhQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEEYcmLFedZgr2KhgSGepA+4YKGb1y774M/HwOnVI3VxhBz1m8RdAXAVZC1NrPRheg==");
        }
    }
}
