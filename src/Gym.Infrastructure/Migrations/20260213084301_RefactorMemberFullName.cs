using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorMemberFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEClqrnMBMDIroIS5hHY5GV4up557lrhItzjPTWR3ZpZWzJ5NT0e9xWwNZf1jtIs84A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIQ4j7xg1hfhTJY92JWRRpzOXSkM5QIkfi8ui52iSzDXFibAfIK/7NgIVqHBR+OkBQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHL/LGKN4L6LsuomdoOVR2UWlR9cEECM/1U0r1IaspG6Ms00PFkRD0xSp/51FiP/Aw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENpNYPFdYJabPl4RPXmLXdJlvJkl6cv3J0Tesz6YSD0fDaJTfsuFsZkaTj+PD6yWBA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFSdS44jklNxxLm8n23HVfRqn8eIU66xV2RR59HqXry7Bbshx2Fv9oOVLEmZzeO32A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELNLn9B2igkqz+ouDxmqg4UWSfFcxnQ9wx6aWnuexeIfHkbdEOog2FTDFo4okhEy2Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENXe+6kAiBCYp2yk3fT+w9mUQX1H5EJp4XIQsQAi+d8pO2AK0TBukm95d8ahQ3Dd9g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF1cW6wfYAMGfqxmTqELLMc9wXuG86q7RB/nutvdJPTLZKzsXffoNGAayQtYBUvrDA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEPB+kw0khm2+zmuaA93mqGST0+qTLg+gOcnlSQgwq07Buojsupi1b+4QxZAexxKUw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELicIUqGOBiqaeDUQH7bGd+JcrvJKhOTqv5v97no2sDcrMPmc5axoABhMYmQ6aOquA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOvEvcLetE/TBQSG0kQtU6vLiV+EuVlLMibT34rBu2jIBrVmO+GMGUeBSSWzeDcDFQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEETxaJ6ZVqjKKe18TQupFAh9HKuim3bQvttTKW8MPMrnhud7po3w4SgAjBLCtdfv4Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMfoXtRovFk0TnyAOy0pgthbLm0Atxa0DNq9VqemORHMJHaAtlqmlMXn1c0T1JYITw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEam9a1GTVnSL0S7Vx9P6aj1A1Ad26Bs4QsNGUhZtBXhk/QWo2MCHgsaM9ioc7kZQw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOr/PUW4vl2jUr2VAIVQU+xcEfeI1qKbKRMxaTU1b87hYc8xWwHmqRA1Li3FRh6pdA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ7PhSKu2GLrC7WeBupK+f4oUO968au14BYKWnYKfWPCLkKi3/4cAcUQys9X+mDnwA==");
        }
    }
}
