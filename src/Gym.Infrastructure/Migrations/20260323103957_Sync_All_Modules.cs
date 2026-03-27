using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Sync_All_Modules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
/*
14:             migrationBuilder.AddColumn<bool>(
15:                 name: "HasPT",
16:                 table: "MembershipPackages",
17:                 type: "bit",
18:                 nullable: false,
19:                 defaultValue: false);
*/

            migrationBuilder.UpdateData(
                table: "MembershipPackages",
                keyColumn: "Id",
                keyValue: new Guid("10101010-1010-1010-1010-101010101010"),
                column: "HasPT",
                value: false);

            migrationBuilder.UpdateData(
                table: "MembershipPackages",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "HasPT",
                value: false);

            migrationBuilder.UpdateData(
                table: "MembershipPackages",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "HasPT",
                value: false);

            migrationBuilder.UpdateData(
                table: "MembershipPackages",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "HasPT",
                value: false);

            migrationBuilder.UpdateData(
                table: "MembershipPackages",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "HasPT",
                value: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "👑 Admin/Manager (Quản lý chủ gym) - TOÀN QUYỀN");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "Tiểu quản lý vận hành", "[\"*\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "🏋️ Trainer - Chuyên môn", "[\"member.read\",\"class.read\",\"class.manage\",\"equipment.read\",\"equipment.report\",\"inventory.consume\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "👩💼 Receptionist - Vận hành hàng ngày", "[\"member.read\",\"member.create\",\"member.update\",\"checkin.create\",\"checkin.read\",\"package.read\",\"inventory.read\",\"inventory.consume\",\"report.read\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "👤 Member - Tự phục vụ", "[\"member.read\",\"checkin.create\",\"class.read\"]" });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(2727));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3603));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3605));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3606));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3607));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3616));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3618));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3619));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3615));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 23, 10, 39, 51, 603, DateTimeKind.Utc).AddTicks(3608));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF1gjyXoif09w84iyQRROxsukYCveXpFAB0S99sp3/lhqJZRJqT7Qj3tOLrAEju6zw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEhbdHKBThGiRqpgRyiRQF/I/fvDQjzUaxXTohxbvwqfdpWMvx7NR38RAafME8oEjw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKBf1Ntua4GObrmTPfnSrnHooaxqAKXFBGEbOeOipVkHE96t1GqSNQTflucObYdP2g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELnntOhyhrb3mrcTOk00bwkLJdI6uV0by8DK04GtsH2lQXGN6Hd8+MfKtYJTOj6FVw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKUv51+RWnCNd3Jbx6yMEo8r7AWUYMKvNj1XdWapP09/ktrtBkx8dq0aL2x8FKlrRA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBrrHXAvj8gAPuWggULBUnGa4zG5U6oCto5BlZBsvjjlZblIPwD7oLfqWOpo8pIHvQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENOzXUl7ARpVabc9r7n0y2qEqgIf2B1FvXIR8RIA/ZxdLx7BiFrZ1WbvBdIOZbTtKw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFnAHo8vTK+SJc/CStD1tsMCQgysKVVLKJB1Fk/Vi5aOc8/f/oZ2j71O6LseQq/KvQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBZo9rL3k2+/w/J8ekN/YA+LE/5SxFqxRNovl8qiW86KudW8jLyIg8zASuefTlyJpA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPT",
                table: "MembershipPackages");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Quản lý chủ gym - Toàn quyền");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "Quản lý vận hành", "[\"members.*\", \"packages.*\", \"subscriptions.*\", \"payments.*\", \"checkins.*\", \"reports.*\", \"inventory.*\", \"equipment.*\", \"billing.*\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "HLV quản lý lớp và báo hỏng thiết bị", "[\"class.manage\", \"equipment.report\", \"inventory.consume\", \"member.read\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "Nhân viên lễ tân vận hành hàng ngày", "[\"checkin.create\", \"member.read\", \"member.create\", \"member.update\", \"subscription.read\", \"subscription.create\", \"inventory.consume\", \"report.read\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "Hội viên tự phục vụ", "[\"checkin.self\", \"member.profile.read\", \"subscription.read\", \"class.enroll\"]" });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 22, 8, 43, 1, 651, DateTimeKind.Utc).AddTicks(3504));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8034));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8062));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8068));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8074));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8154));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8159));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8164));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8149));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8080));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMrrmqY7il+mVPSfzKvWYwwn6jyyJIO9j+0RjaAJXP6EdWo3+rdoDwF/PQOyvknuVw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKLeEO7/FJ5SJBvXWiwIvObQ+fonizVgkoXxxnsEHiUnkFOANQu8HmZS3URiyz6ZvA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC/0Z2o1lKv1M3MofbqIQlnSyF6bvIMunuByfR8eNYQZ4qSteOenc5PPkFvBNtjBxA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKzfTdca/IKsSq7os+5SO7KfKdE+/W9pqBrJwCwIQdLCw3vOfk4gi23SaogfmTz38g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDzoR3qrlPFgeOziV8yiUZgaEpJ8/qWkgwHncFPdHwpBznybMhqwBHrPUJq+JGBwsQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC1a+Pn1Ii+PE19ofALeUCtQXWx7RED+DVEgFfmL929ag1jo/iLchssIX3kz/8Gjyw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOa4HfRKa+2HRh3+OT7apWLu9nYcuUmjw1jafbV9tn5uT75wPyYIk0u3wtAnYO+71w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKkNajLEIctqwbAP04xPfpmFNRXvX6NEYN8DiWmvgj/x9dVh1eKJrvDTJIKDPPk6Sw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDzXsvG/6MmwSDiXLTnkX4g/EHhUm4aavZQVZZheJbenCpLdkdyZXMSZsELzRwfL3g==");
        }
    }
}
