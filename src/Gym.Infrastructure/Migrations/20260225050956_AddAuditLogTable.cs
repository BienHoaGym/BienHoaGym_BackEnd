using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECj+HHSfWe/jGQbx9XWxkt9Ac1/u2QjiL/txW2PWPsi5Tc6BpKk9OHMIVq0CGzcOYA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM1Um85psFCysR1Su5n3JU1Pw/9c34TzITOoaGG4SUdtbHoJeW36Ok2mYnpl8rHa/Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMUNTAYneit88U63q+pQQC3QxCPVOCEhTIPMlOdhs7Z4l5lPjxBqIdxV8vi01FVa7w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEgG+L8iV4AumXa4dQSUBqfFFw/j2YONm/xioP5zoGknobG6Jut5qp2NCUcOgiIMaA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEu6jsb2yJlubiI5S0qA3NwE38TRXH7kTtq5C/O1tl7CU0Wi5pJ2DahE7KM0i75dig==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGbTqBWqqZGUY9FI2Qm+en0DvcP0arjuGBplbCuhklkJj1marBEagFEvUVHjUi10ng==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN0L6sZ7VN2/mAmCSEcmr5Gym0Juj7zc8U/omo8XNhIbi8O6TLrre7szu6IuAC1BBg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAhcJVDAQcTk0U5wxiY8RXKahdheFXTeK0XdmsMhlVBqVmh3of4bfSihNUIHQ0ACYQ==");
        }
    }
}
