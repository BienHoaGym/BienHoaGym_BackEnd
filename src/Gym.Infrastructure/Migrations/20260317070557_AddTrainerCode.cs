using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainerCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrainerCode",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "TrainerCode",
                value: "");

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "TrainerCode",
                value: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainerCode",
                table: "Trainers");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAHiowP1XuD+MuTt4+8yw6LVFzqo61nog6WqXtKkSKZtTn5OGlaS6Trl1h3sBAR6hw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENk829DXRC5n6qOhY0fDOF1ah/a8jy+KQEIx7AwzqE9ifZasqdyhRH+wHoLt+KUGZw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM/wSlohaIJLmXsxVk+SgfpLV1xwAb0E2v1YX0+D8kJRJGrLJaS/1xBqVW2LgtMacA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO1vFUy1WdeIelJpg1C+WZRU+agOB6EdflSWygUZIClknXvzvc4mtiVclQMWY/x/og==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO+CMY4KuMxxOrxpnHcOWvr3OjCMsHiHr7gR73Iq/AdNZAv87knApWnb7lWHdHuyhA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDkFZU6S7EDLolWRXy974rxQJRw22JZhFGFV1D4sUm4d7YwYa0Mdn7+7cWWzK/X7qA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOUS2s1YXve1sWiuQVwKp439fOAgVrep2cslLEpg8/dRRzFtObxmG7hdLP0OhVkzOA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF5WvSSMl0Uy4y7OR6fbJeBjnQs+K0XU7ahD9zsnDGdwoXXjNm+hHhCR+kQo6wnWhg==");
        }
    }
}
