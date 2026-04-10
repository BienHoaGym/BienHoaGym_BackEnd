using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffHRProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankCardNumber",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HireDate",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 9, 7, 17, 538, DateTimeKind.Utc).AddTicks(7453));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 9, 7, 17, 538, DateTimeKind.Utc).AddTicks(7461));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 9, 7, 17, 538, DateTimeKind.Utc).AddTicks(7464));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 9, 7, 17, 538, DateTimeKind.Utc).AddTicks(7466));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 9, 7, 17, 538, DateTimeKind.Utc).AddTicks(7468));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 9, 7, 17, 538, DateTimeKind.Utc).AddTicks(7475));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 9, 7, 17, 538, DateTimeKind.Utc).AddTicks(7477));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 9, 7, 17, 538, DateTimeKind.Utc).AddTicks(7500));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 9, 7, 17, 538, DateTimeKind.Utc).AddTicks(7472));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 9, 7, 17, 538, DateTimeKind.Utc).AddTicks(7470));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Address", "BankCardNumber", "BankName", "BirthDate", "Gender", "HireDate", "IdentityNumber", "PasswordHash" },
                values: new object[] { null, null, null, null, null, null, null, "AQAAAAIAAYagAAAAEGwwGA9+wlkg2LXdcLwzv+G2NKrO4aPmWUnMqr+WJFGVl1JDObRx84Di6HLADzdIKw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "Address", "BankCardNumber", "BankName", "BirthDate", "Gender", "HireDate", "IdentityNumber", "PasswordHash" },
                values: new object[] { null, null, null, null, null, null, null, "AQAAAAIAAYagAAAAEBKJ7JV2jaHii0lRD8IHyOGtrcMSC9Bi/92+xRduOUbmyaNcj024u7jM6ui+/dRE5A==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "Address", "BankCardNumber", "BankName", "BirthDate", "Gender", "HireDate", "IdentityNumber", "PasswordHash" },
                values: new object[] { null, null, null, null, null, null, null, "AQAAAAIAAYagAAAAEMCtCyKfXQLp1q69lHnm+8sjRlN8qXjC55ej5h9CZBaqCVn44FsZzT9hxEYNlnIo8Q==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "Address", "BankCardNumber", "BankName", "BirthDate", "Gender", "HireDate", "IdentityNumber", "PasswordHash" },
                values: new object[] { null, null, null, null, null, null, null, "AQAAAAIAAYagAAAAEBaeWp+dbEcKPwFzaRm1QhJZyZXKMgV0Ijx5I7AUTPWLm+0bcahCK2tjNxfH0GliZg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "Address", "BankCardNumber", "BankName", "BirthDate", "Gender", "HireDate", "IdentityNumber", "PasswordHash" },
                values: new object[] { null, null, null, null, null, null, null, "AQAAAAIAAYagAAAAED2evMrC0UvnDW0wHY9UWZw2PK8FdsneDW1hCmoLYEzz4sC/EzidY3VSQLLqrUYh1A==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "Address", "BankCardNumber", "BankName", "BirthDate", "Gender", "HireDate", "IdentityNumber", "PasswordHash" },
                values: new object[] { null, null, null, null, null, null, null, "AQAAAAIAAYagAAAAEC99nXvljEaZaicpfsOMVM7NHujO9yG1hiO1OXFJfzUNO82K9Knoxs4R5ce8iKdYTw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "Address", "BankCardNumber", "BankName", "BirthDate", "Gender", "HireDate", "IdentityNumber", "PasswordHash" },
                values: new object[] { null, null, null, null, null, null, null, "AQAAAAIAAYagAAAAEMdfgw0eKhQAkFqLx+Pb56ItS3RZNkj8oPkKwR/SQM9H6ozcBJLuhm6ulfa83LmntA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "Address", "BankCardNumber", "BankName", "BirthDate", "Gender", "HireDate", "IdentityNumber", "PasswordHash" },
                values: new object[] { null, null, null, null, null, null, null, "AQAAAAIAAYagAAAAENkEuVIUA22icsUY229HnqOzINyyWpdqBiVsAxnwhGolF7xEWFDDCjkl27CCIW+IPg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "Address", "BankCardNumber", "BankName", "BirthDate", "Gender", "HireDate", "IdentityNumber", "PasswordHash" },
                values: new object[] { null, null, null, null, null, null, null, "AQAAAAIAAYagAAAAEAX7dQ55BmNyJ1aWqREPwL9rHvABfUic/9dnhuLWN5QEwU8hs+bVciX3oqm1cEZSbQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BankCardNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HireDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 8, 2, 15, 896, DateTimeKind.Utc).AddTicks(4451));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 8, 2, 15, 896, DateTimeKind.Utc).AddTicks(4461));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 8, 2, 15, 896, DateTimeKind.Utc).AddTicks(4463));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 8, 2, 15, 896, DateTimeKind.Utc).AddTicks(4465));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 8, 2, 15, 896, DateTimeKind.Utc).AddTicks(4468));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 8, 2, 15, 896, DateTimeKind.Utc).AddTicks(4474));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 8, 2, 15, 896, DateTimeKind.Utc).AddTicks(4477));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 8, 2, 15, 896, DateTimeKind.Utc).AddTicks(4478));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 8, 2, 15, 896, DateTimeKind.Utc).AddTicks(4471));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 9, 8, 2, 15, 896, DateTimeKind.Utc).AddTicks(4470));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEItj91k9msN+CgfR42bBN7O7aqCQl1nk0SbXl74Lb71xJc6u8jsPDo18o+bXg5zaJg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELDJkoJ3AAxPTyYLNnFOnCXR865ulj3OsTG5zjTBZjJ8ZVsz3BHkZQ3BLbY1ebVT7w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJYqaOsfokXGcr9szI3AMFf7nDE8iZunA904LnlVH5KgXojcPF7wHoJ2V+NhlLQ0fw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAiNmnhU220iLVvRVrR2N63t1r6WYAu+uPydTg+3nZ/1l3Q+jGCCcY1LSWMMqQ97sQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELRswg5kmRB5XEydffDKaqI0WapsICPDIT4CjJrX1GG8SpQiWnwJtoRw7N7cqR3l2Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJMSPZn+flMFA4L0IqyviEJhdhPP2K76T2wAfXZUZTNOzpUwCygjo1qasS/e+ZBLCA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMQI8WINAFebYP27dfBJ5i09WUy7PTFM/nMgjVkaQ+mj6uqW3ON56OfBeiBzVa8aEg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBhVPLqACcsJAVDA/LKiL/lHYD4cAoI8DF2uNfBaBv0B7S2hPSFKNvLxbyeYJFIR4Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMlD5Fov9GwuPpbg4i0TR8rVV9aRSaP5hoWEbIyywqPNukxOtQoXdHKrFhgYpaKjeA==");
        }
    }
}
