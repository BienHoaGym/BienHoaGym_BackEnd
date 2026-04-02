using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQRCodeToMembersFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "EquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"),
                column: "AvgMaintenanceCost",
                value: 50000m);

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
                value: new DateTime(2026, 4, 1, 2, 18, 46, 352, DateTimeKind.Utc).AddTicks(7958));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 1, 2, 18, 46, 353, DateTimeKind.Utc).AddTicks(4198));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 1, 2, 18, 46, 353, DateTimeKind.Utc).AddTicks(4228));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 1, 2, 18, 46, 353, DateTimeKind.Utc).AddTicks(4231));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 1, 2, 18, 46, 353, DateTimeKind.Utc).AddTicks(4233));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 1, 2, 18, 46, 353, DateTimeKind.Utc).AddTicks(4451));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 1, 2, 18, 46, 353, DateTimeKind.Utc).AddTicks(4478));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 1, 2, 18, 46, 353, DateTimeKind.Utc).AddTicks(4491));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 1, 2, 18, 46, 353, DateTimeKind.Utc).AddTicks(4430));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 1, 2, 18, 46, 353, DateTimeKind.Utc).AddTicks(4237));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN9Sm5hb3HBHug/SuXxJh90muSGUVfpkKyZw6oEdpP6C+sKzo+8/87vepYy2E8A5oQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDAe0DiuGP54sz1dxgMZ+rbckCLQ824/s1Fw7WSx+IG+Fxw9aRT8AobzsZvyC7/ngA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDWR/icOaCNPqH/4NvvbeEfrw+AVlPPBEUmeRex8Rza5rx6MrkuQVA4UFyHGB3EeKQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPXMSUlFs6/WHVzya/P1eE/kL+e8ju2qFDKcf7RmI8lQbuQ3hs02FvhVOhZYHTbqXw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJTaKVGmWdSVNUHC+M+mgX15LUPiUCrgRYQp3O6VZNc79MQsK2KlWNyUbfAMCQs2fQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELRoCpeYkCZaspX8MQ+nH7wEGfJ1P6hEUCF6QGBisUO5U4mRhuU33tKnyw1iuRutqg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPCoFI8280uK3dXswnYkE688Y0MStLLtLOHGpTbFl4cBSqSJXxIPmMYluI9KL81alw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIoiU04lgD8hFiYCzTLaZT8/+sKzejDAow4zOqIv3tzeTBgYRdxBX8SViKDbcVh+Cw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE2g3+lQaZgg3Ton6rJ9PDJCinVv9hIOg/uNlSpDiJE24wtlk+lmHrk5egN/gVnafg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"),
                column: "AvgMaintenanceCost",
                value: 5000m);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                column: "Permissions",
                value: "[\"member.read\",\"billing.read\",\"billing.create\",\"payment.read\",\"payment.create\",\"report.read\",\"package.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"checkin.read\",\"product.read\",\"class.read\",\"trainer.read\"]");

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 5, 34, 12, 368, DateTimeKind.Utc).AddTicks(3524));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 5, 34, 12, 368, DateTimeKind.Utc).AddTicks(4192));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 5, 34, 12, 368, DateTimeKind.Utc).AddTicks(4194));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 5, 34, 12, 368, DateTimeKind.Utc).AddTicks(4197));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 5, 34, 12, 368, DateTimeKind.Utc).AddTicks(4198));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 5, 34, 12, 368, DateTimeKind.Utc).AddTicks(4216));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 5, 34, 12, 368, DateTimeKind.Utc).AddTicks(4218));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 5, 34, 12, 368, DateTimeKind.Utc).AddTicks(4219));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 5, 34, 12, 368, DateTimeKind.Utc).AddTicks(4215));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 3, 27, 5, 34, 12, 368, DateTimeKind.Utc).AddTicks(4200));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENKyr7J9AnZrGvwfWij2H0k0xcsoThgnAgIr7EQHggsz+jwkGHW4yeNakqS73Cc6ow==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOUVRe1d3iSPCqkRyr5kSR3kDL7gAVF3OVev8xpva8BwFfeedeJ+8qjG/ii94foe9Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJM/e8N/9TTeU1EOqI9vb7sIYvuL+3pYKbRdBGQuo/jJsS5XqUZ8Gm9G3mz6ygd4EA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAcy7bUBP4dHbCgz5aCkYRp9HdUVgJwnOzBj8CzWMvIojTTAYM6OlyFGfFUDJjuZ3w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIX86gIuzJWIPUXgjROiBajjw17TzcsDQ2v+dUdYcbg7eCP8tSEQqsCQBeHeI6Zo0A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHlwM9rFrIE+4Gz8u3vT4oDR0ECMzxJQUAw5jb28NgL1v9IOK40I7/PRYoC5mPKLHw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHADyOmXgWlROz+wx/nCoVcJcMQpbO6tx5Tkxc1NhhNKvf4eis4tB9HwDPGBQErmsg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEML8/bQ4pTSdcyxZn66TOY7b3NlGEYiDMb6WowyC6osQnBAu1+hsiQqCVGzXwO31/A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELpB9XwfLN1x1nIdqTVey8cF7KoxhqlAxygJPeWFETMVr+8WwM+RIC7Fj+soLY9umw==");
        }
    }
}
