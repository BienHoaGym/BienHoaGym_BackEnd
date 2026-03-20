using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSnapshotDataToSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountApplied",
                table: "MemberSubscriptions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalPrice",
                table: "MemberSubscriptions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "OriginalPackageName",
                table: "MemberSubscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "MemberSubscriptions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: new Guid("80808080-8080-8080-8080-808080808080"),
                columns: new[] { "DiscountApplied", "FinalPrice", "OriginalPackageName", "OriginalPrice" },
                values: new object[] { 0m, 0m, "", 0m });

            migrationBuilder.UpdateData(
                table: "MemberSubscriptions",
                keyColumn: "Id",
                keyValue: new Guid("90909090-9090-9090-9090-909090909090"),
                columns: new[] { "DiscountApplied", "FinalPrice", "OriginalPackageName", "OriginalPrice" },
                values: new object[] { 0m, 0m, "", 0m });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountApplied",
                table: "MemberSubscriptions");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "MemberSubscriptions");

            migrationBuilder.DropColumn(
                name: "OriginalPackageName",
                table: "MemberSubscriptions");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "MemberSubscriptions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOsQ7h6lKcpUIhjoBSoj71Q700WAlwWUlh3rWlCxs3yST+ah2FtN9W/18oR5WBkvZA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGbs6ae9gvjxjBmqFCarwnEZOU9lmuGffC5WiaLrPxnXxKM1NG0ClgLFlyOMb6oCuA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI1+k+tDg0XAWxHckkpDDmr8lHZTLJFxN4qxKaPhwEz0oMd8qtGvXkqZiFtKxvR4fQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBlmdkk6vM308KIeMYMEO1BoHBSOdtZPyR9osX5zg+MJBZWDlttwlsyU9LXCisMwtg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFqUFKuGPsmF5jL7Q8ojqR6f6+30aJCW8hSpD7mMLsXZ1wLPpeHudQQUDcl3xaWZ0w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN9rnOOnqmXNh9J49FpQJreQplYshbo7GVyPxnT+Irt0vFF095dTo0SLSZuiz+LWqw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMwr9RGDFuqMQ/zffuvY3WXnPOEzPLvyta5b+3zj0Iu3VCVbWNRQb44iaguOGzVv/w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA6SHbvqdlXWXLxPzoNYZR/1Z87+P1gI3/GXV1B3NoWN+9JlL9mUB6tb8MernQJgWA==");
        }
    }
}
