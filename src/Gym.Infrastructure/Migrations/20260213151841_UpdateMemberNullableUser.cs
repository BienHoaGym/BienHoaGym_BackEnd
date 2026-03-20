using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMemberNullableUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Members_UserId",
                table: "Members");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Members",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                table: "Members",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Members_UserId",
                table: "Members");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Members",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL2mJLpVyZvDqyFPdhIf0u1v6JPmOAInUz8EdBOWNifnzo0Lll6ZIAzPac3kleHepw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDrkBy3SKIM8h98Nd1ZWGstt0GbFqqmVMUZ2kt3aFX7INttHPxoq4733twhcWF4Rpg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDRnUaWx+5a9/OwL6rSKPsBQWhmjEjpGiQAZAMd+WVKe48jSscwmu0sG/pZdqFACjA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPRxMPcRlvVBikSwUmI+wR9fmyJHHTqmAZBHZYvIHcS0CtFAv9mCDlUGRSvtm9cPAQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH7/G3OUTgZ9tihAfqt9fPYmnHvD4w5vh8E4Arh6flshYFzHahhtXPpeSNMfcYMADA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENeK8qJiuyP5pUCeLbK35PNUn8NzBj7DU+87rPp7BcfcpSCc2qEbe1JoOaB2lq57Jw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED6hBr9jHduo1s7d51s+hmpY48WDpzNqQ7gztuS7EGHy1+tnYzsrzn9BiiYYMptyGQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFe/tyyxmFCVfm0yLKU0BeHtqHEXr5E+D7P4mePWgrAzQF5rBzXL11rLUQHZzfy8HQ==");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                table: "Members",
                column: "UserId",
                unique: true);
        }
    }
}
