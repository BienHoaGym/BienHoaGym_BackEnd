using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClassType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassType",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AttendanceDate",
                table: "ClassEnrollments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAttended",
                table: "ClassEnrollments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("50505050-5050-5050-5050-505050505050"),
                column: "ClassType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("60606060-6060-6060-6060-606060606060"),
                column: "ClassType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("70707070-7070-7070-7070-707070707070"),
                column: "ClassType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ+q+FSgBFuKw6YQaHb1HrEfy2eVs4ZtAaQpKdg+w6gkt2LyPGjH3+QLQuRbokUyrQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELiazw7K1MM5HUPkzDGq2A735Z2vtNrDg6+wh3ghTb6udcLIPAzENBl6ZiaPeeU9WA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOXVP1swWg9NUlB5A1xSPM2dUOssE7nzV1qNZWOxkFmvmEQ2jbuPhyTCira97dIZAw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOHvYnwOKxu9N7ca9aSapSuQie0Xh1+5An/vOCjWwebO1FqWdFFm8eaka5d0OgdEnw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEME/YBnJmp/guFuGrWon80GLpSx71jVE1O+VfiJVNiKI5bgpwymSwVo1bHGwtBVDRQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENBhgRjhJhD3Er7oKtvwW6ybX7cHM9lGGrsTnQdE8XUqXxe/fE6OuRBT3zcvhmAs0Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENOJ9MgCIYWnzimbQ5SAN0u5X7NbWorKEIsCmTQNTgMJ6Da4hUeHoznFB8otS9Mahw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELFnQtM9B0smCCoA85ovPMk+Br1IrAK2R70vHPEXUlDOkLHJMOiCrE0lbrGu2zoWTA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassType",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "AttendanceDate",
                table: "ClassEnrollments");

            migrationBuilder.DropColumn(
                name: "IsAttended",
                table: "ClassEnrollments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED5OnqhCpYpXOSXya9em8zEAWishPGIUOVcswG8w1Fx9Cco20bfanhP/b8GnyPCMxg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAQ1caH7DXv8WRyvV+Wca0NzgesXEas+xBbGEoIC7kV6Ct69Lp9KrcuCjzUJ3az5Iw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGpfpGc2r4NE32u/uI3oVUKVubuLGDqL3T9qGQWeGyoU3+RNFSLNqjJOIsetz3MEbA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBYfs31xQyl7GeS8ZYON2SNV0Q+VkD73QlKhbGzOknSwE6YbsNiAeVACiXho4ljE6A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIYV86c4zbtfK5BINqV3yVKVPx1tupUN9STK27Z+Id+nhQhuhDt2oOP9i1NAlPdngw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECkDjJ6yEakQW4cGlzCodEBpD+7/66uLzjg7YK8IoB3EKamG2AmvCTkxtTgURrHzlQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH5AlYFA0pF9/ONmqE/EPZ3RUxyXaLXxz84pzg8wA8EWnHtMHM1bbDTOaMIdzhdP1g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKPl7aH5qaKs0ly5gpbmRnwQDT7IGwYmHSc3ixJatd739QEiaGry9uJLl797YUzzUw==");
        }
    }
}
