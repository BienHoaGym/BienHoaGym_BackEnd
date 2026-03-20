using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class memberdto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJm7LTIuTQub2VXLFNdtHJJihD3veTvwDnbTTIPbRZjrncPh4ouO5QCztALsSW+Oyw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHmEYV9prAwMIoxCNDRxcZfKaKudjbd1SSM+stIzPFi1vidxWAgGUNDiMgJUErPahQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECx3Id27Kbhu6HGYtiboKpvKgOnRHt/9EHMS68i3v087TSi0HayRCsreZciwmX1/+w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ3ozhXQC223BTfZQ2pOP4nFO7aRorFvoob0tx1/n9FB+0H9txeA+8Z3EsUeVmTxnA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOGpFvUTf5q+kziDKJdKK+DZpeae5+QKDZdMCnYE0jDV2frnioLKb1zMfYn4YJsdHw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENpgA01P/ngMPjn43mJhYCdPgrBA3tW4Cr0p5jq6Vldy9Ue6UWoR08m6JeKcV8MIKw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFUmbY7DFfa6rtjhecz+ACYUYmE/Xlbn88hNz3d9re0/OAkUUr/BG9Sgiqh9vM8mTQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOME/9hJ3tG4mW2937bQytVErggV+rZkuSHgQ8vtxxbyjchWOoulvj6wMXjpJyzqtw==");
        }
    }
}
