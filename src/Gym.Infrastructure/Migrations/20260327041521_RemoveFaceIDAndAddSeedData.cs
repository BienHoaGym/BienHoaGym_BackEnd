using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFaceIDAndAddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceEncoding",
                table: "Members");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Permissions",
                value: "[\"member.read\",\"class.manage\",\"equipment.read\",\"equipment.report\",\"inventory.consume\",\"product.read\",\"trainer.read\"]");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "Permissions",
                value: "[\"member.read\",\"member.create\",\"member.update\",\"checkin.create\",\"checkin.read\",\"package.read\",\"inventory.read\",\"inventory.consume\",\"report.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"payment.read\",\"payment.create\",\"billing.read\",\"billing.create\",\"product.read\",\"class.read\",\"trainer.read\"]");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "Permissions",
                value: "[\"member.read\",\"checkin.self\",\"class.read\",\"subscription.read\"]");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                column: "Permissions",
                value: "[\"billing.read\",\"billing.create\",\"payment.read\",\"payment.create\",\"report.read\",\"package.read\",\"member.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"checkin.read\",\"product.read\",\"class.read\",\"trainer.read\"]");

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 15, 13, 111, DateTimeKind.Utc).AddTicks(7625));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 15, 13, 112, DateTimeKind.Utc).AddTicks(70));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 15, 13, 112, DateTimeKind.Utc).AddTicks(77));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 15, 13, 112, DateTimeKind.Utc).AddTicks(78));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 15, 13, 112, DateTimeKind.Utc).AddTicks(79));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 15, 13, 112, DateTimeKind.Utc).AddTicks(105));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 15, 13, 112, DateTimeKind.Utc).AddTicks(106));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 15, 13, 112, DateTimeKind.Utc).AddTicks(107));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 15, 13, 112, DateTimeKind.Utc).AddTicks(104));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 4, 15, 13, 112, DateTimeKind.Utc).AddTicks(80));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFJfrhixFSbFKr45t7oLAReKFAuABcZUy1oVB0cR0QOhGUQLmKZT65xHPTaTMAhW3A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGygD2oweK0fTPDgJII7TCzOr9F+R6ukBKV+dA6hPdcOHHjjXhHjD9Xl+08N2mLmIg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECWfn/muMIO1sGBQQLydxzwu3FUGV2Z83tT1+HThmwwZHiZe8GMnUtZK1Du+QpVF7A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEESIRm4EQU5sdXZRnQjEuHj+Q14wFuhsUgN0QjzLEyOy1+6puCfoI9HcM909+Y9UZg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMCZp/xvRaF1CzjaB1i5sSjZPyqiIGAGER8HOpf/JQCjd0AXi/f9PlC+/pyov50cPQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBR7ulirakrlmiIUb/JUyDr9IqzSz8iKpJwdrPLD1LfbZYHhZBeessJsTMOO3Cu0nA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPRv3q9qb9gBe1E16HGhucro3bWsaTGjI1PmUmpf1Cz5aFjRvw1reDZCJ/qLzivZ9A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENs8WOlzUn48YXnBZYuuqVGelxPuSzsLFv4igd57yJF2yhDJ3hHIh9sBvuZC9mgeZQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE2znGrlr/80SIKd18HK/NspsQdqE3TQ+ScU2go3uh7psGRr8FnZMU7edxHJmJPWHA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaceEncoding",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("20202020-2020-2020-2020-202020202020"),
                column: "FaceEncoding",
                value: null);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("30303030-3030-3030-3030-303030303030"),
                column: "FaceEncoding",
                value: null);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("40404040-4040-4040-4040-404040404040"),
                column: "FaceEncoding",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Permissions",
                value: "[\"member.read\",\"class.read\",\"class.manage\",\"equipment.read\",\"equipment.report\",\"inventory.consume\"]");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "Permissions",
                value: "[\"member.read\",\"member.create\",\"member.update\",\"checkin.create\",\"checkin.read\",\"package.read\",\"inventory.read\",\"inventory.consume\",\"report.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"payment.read\",\"payment.create\",\"billing.read\",\"billing.create\",\"product.read\"]");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "Permissions",
                value: "[\"member.read\",\"checkin.create\",\"class.read\"]");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                column: "Permissions",
                value: "[\"billing.read\",\"billing.create\",\"payment.read\",\"payment.create\",\"report.read\",\"package.read\",\"member.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"checkin.read\",\"product.read\"]");

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 43, 17, 74, DateTimeKind.Utc).AddTicks(3116));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 43, 17, 74, DateTimeKind.Utc).AddTicks(3744));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 43, 17, 74, DateTimeKind.Utc).AddTicks(3746));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 43, 17, 74, DateTimeKind.Utc).AddTicks(3747));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 43, 17, 74, DateTimeKind.Utc).AddTicks(3748));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 43, 17, 74, DateTimeKind.Utc).AddTicks(3794));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 43, 17, 74, DateTimeKind.Utc).AddTicks(3795));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 43, 17, 74, DateTimeKind.Utc).AddTicks(3795));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 43, 17, 74, DateTimeKind.Utc).AddTicks(3793));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 24, 6, 43, 17, 74, DateTimeKind.Utc).AddTicks(3780));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKGWdP2HG792vlgwPNHWtVGlpz4WbY0PgUmbkvWqDo9uWkryGlv3RbaiM7Iai2D2XQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHHwPJJs/3dif1yXLgPPUcnRjmWiow5UhhZ0xa5m2RFmSni5ahp1ONZW3loJaIM7ig==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJGQoSVQqGczyO7aSO3VTdFx8MaP90+0T8F/aDH9QhhDkCpXnU74c6jiTqfHs29ShQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDyM3V2QaiuUX/gedJ1sn+UQx0bcasq0cA8z66jgghY8TSvRYxIJDDyE/NHdfsOItw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECOjVghZsSjIbW19PGzL2EgaGSgJWMV2OD0HB2gLCEnSv/gTL6phwul7urjMe/q8VQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGpItuQSygv9+f8kz8e5XB7rP90HSauwBSfKhwol9Znikaqtyf9g/jD17NqHHC2t0g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBRStnHFDwa061y/KS7rrmqLnr4GShZuXfvxUx2XSIR3DS9R1EslkMVMEZFX5YJpcQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA+iVKCRJkKgryOLAmbJE2eDEu9BoE6wrH3nX6cpA6NEWtuJ0+TR2M7Jsdhg/wJoWw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPZxRfbsaDPdY6S/IWrBySq0YQLl1tLh0bVU2NcGUqEyGPLMr3vBOFtaSS7OEa/6iA==");
        }
    }
}
