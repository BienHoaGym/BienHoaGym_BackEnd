using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainerSalaryAndAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "Trainers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SessionRate",
                table: "Trainers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "TrainerMemberAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerMemberAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerMemberAssignments_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerMemberAssignments_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "Salary", "SessionRate" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "Salary", "SessionRate" },
                values: new object[] { 0m, 0m });

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

            migrationBuilder.CreateIndex(
                name: "IX_TrainerMemberAssignments_MemberId",
                table: "TrainerMemberAssignments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerMemberAssignments_TrainerId",
                table: "TrainerMemberAssignments",
                column: "TrainerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainerMemberAssignments");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "SessionRate",
                table: "Trainers");

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
    }
}
