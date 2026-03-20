using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProviderLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "StockTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELiVn9PpS8wpgifBJEib6rVEc4elMwEuLVcZEEUWRMTs/f/IhKftRjh+Zo7OGpSzOA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECazTxytWv/Cfa3RjAKNrH3JSW15HCrnkPa2LSkQZNCJ30pKQZ/EBdsxbHANbQjxkQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM1WMnhl7kAgCzYm8HG/lDHMQ5uCBn0TeKIqPiESDCixR6GrL/ZZ72SYiah3sn7CJg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOQdLTfNJn/SgeE3gCLjsO5xSOzlUbSXNgN+R6m0XYxq+uMVmXsHUov2hLXgIYsXrQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH4oDFfWABcIKm30Hiplj8tKdCovAqYCS23UWxYwnUdsWJD4C4iEjrI0vAtK4UH4pA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOTH+ARbaPj8Kuv6lG0yLYjT6LKkRWWS92iTjCPEsk+rsmu5Vf2CUZ0j01Z2hfay4Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIhu/utwb0zcpYJ7KmkcCVCNFZtgSLhldSvqJoR8E+/E8Al+bh84hAFhOsOHUr2cLA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECGZKYrbIvfhdzqTUYOKV7RZeWPT9XHLlMk2wUdFhQV1aqNtFrGIzh6yZjTx8sIRbw==");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ProviderId",
                table: "StockTransactions",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Providers_ProviderId",
                table: "StockTransactions",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Providers_ProviderId",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_ProviderId",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "StockTransactions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPta/Nn3K/A0zEgfNdAoaMKZwB9zWowUHt2DPb/VT5lmc5eFrTG+PWfINP/hvCMx6w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEP2227hIW4g5A3R2zL+1KWh0XMetoj3SPeoSMCAX0Gs/54+NCpPwgwn9WeYfv4XbBw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEACTWP2D5rRLHxkBKPVg4NTD+ORelJy7b8qIiCd/JlihAewpnLXMDk9KWy7FBGC6zg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEPgj4PzlvVhCsMj6+t9G93Vynhmx6vbB/EnwVkGfFhNlGEeylOPV5rygA8yqvZfwA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMi8eAEIncC6tn5Dbkn7LZDTBKcJmAnv92fWmZYg4y/idWKsPRc7EpUWbTJKCaSVyg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAoeqMiglGwhvUrS+oO/sHNuJidvtaBRzV2BUVSf+lRMYNIg+77Y2i+LTwQnejqhJA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDLxk1957LKJwrdtfQKrWWpgaAV0jQEUrETHNkavIlOOvFF0we2QxWxlHojzkYrxKQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECwM1upWzARfdDXJfaw4ptuMXnJGuX0d2DqsD+47B/gFOxm/40SUnZu5GEKiKbZ0Zw==");
        }
    }
}
