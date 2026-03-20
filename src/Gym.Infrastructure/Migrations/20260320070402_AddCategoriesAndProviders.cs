using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriesAndProviders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "Equipments");

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "MaintenanceLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Equipments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "Equipments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WarrantyExpiryDate",
                table: "Equipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EquipmentCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvgMaintenanceCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StandardWarrantyMonths = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VATCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProviderId",
                table: "Products",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLogs_ProviderId",
                table: "MaintenanceLogs",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_CategoryId",
                table: "Equipments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_ProviderId",
                table: "Equipments",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_EquipmentCategories_CategoryId",
                table: "Equipments",
                column: "CategoryId",
                principalTable: "EquipmentCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_Providers_ProviderId",
                table: "Equipments",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceLogs_Providers_ProviderId",
                table: "MaintenanceLogs",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Providers_ProviderId",
                table: "Products",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_EquipmentCategories_CategoryId",
                table: "Equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_Providers_ProviderId",
                table: "Equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceLogs_Providers_ProviderId",
                table: "MaintenanceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Providers_ProviderId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "EquipmentCategories");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProviderId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceLogs_ProviderId",
                table: "MaintenanceLogs");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_CategoryId",
                table: "Equipments");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_ProviderId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "MaintenanceLogs");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "WarrantyExpiryDate",
                table: "Equipments");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFCqnLKXASsTCkEyIfs7IBNeqDUSx2Umg9sJPPrnU3mYegH57Ps80XPWRbszW4Hx6A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ8jXhr4evdrxCDqR8+QEE6BUWUQ3k2C7wYoY1ytiU+kjpXl948skzG/jzk0QKENmQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO9HMw6LGVLbL2yy20u7VUYupCBsJFsuZ3mgL9FH2RN5iVnY+HU0y9Xn7qfWUguhRQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMd6McneFwFq28o8r4enQpiF/63/YfJMg1Tha4dw4KzeWF/HVPK22XIkdE4Unf0q/Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOr+EnQTI2YzsXqZA8mANjUL134OyFMELDp2eyLdjOqAoejaMaDCH6iRTLImoboEHQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF+2/5kB+aXFueaaR/pzC1fqytziYoqL8n8/9bn55kRHNLBc0gr9zmVzv8dNz7Ra5g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL3tXlghERJtqvxTDdK/F3Kt2KHrc9/IP2eVFzYqvv7YZNXD3/iHmfeWPjoQmHNw1Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMV5nUGpEUs68Q4DTBljnAiNDZshW6uAjNaF6vGqY8FPJHYZSH6M+mufhfFame/uTw==");
        }
    }
}
