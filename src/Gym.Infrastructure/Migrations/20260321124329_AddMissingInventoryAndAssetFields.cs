using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingInventoryAndAssetFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"));

            migrationBuilder.AddColumn<int>(
                name: "AfterQuantity",
                table: "StockTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BeforeQuantity",
                table: "StockTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PerformedBy",
                table: "StockTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AfterQuantity",
                table: "EquipmentTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BeforeQuantity",
                table: "EquipmentTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EquipmentTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AccumulatedDepreciation",
                table: "Equipments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullyDepreciated",
                table: "Equipments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyDepreciationAmount",
                table: "Equipments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingValue",
                table: "Equipments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "MaintenanceMaterials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaintenanceLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceMaterials_MaintenanceLogs_MaintenanceLogId",
                        column: x => x.MaintenanceLogId,
                        principalTable: "MaintenanceLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceMaterials_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBxG6S5l20B2txCZWP+/JNbS/XHRlZL5CfZsv9FQFlOt1k6vKr+H05YA1j+NoWsIqw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG3CyccCA8p+Hp2Cm1RauhqEPjoWWlh32qyIJ2W/nn3MofVxeFIdnS3Q13k7T7+3oA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAIIdDt9Q7bWqmFmhGGceQkAK4OydOYSeDlvXD55ZhUjpgSVIwtmWj+gM1LEp3KLEw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED17rIwkm0NgswWKXpFIyINgjksG3D0s/00jJMtZnEMsWrw0vvEAEUWxlw3NKL2V6w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGyODp0ZpkaE0ivoktrjr2/Yh4SMV+YP9TOikH2nSGXF52H9ZsRvoLZTqZYrA0Cmyg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEK/C3WPEh3ELM7KaURHgCCaUqGrW283IdJ5wW/6ZKVPaijV5wlB4h6jeLWGzBGs3Dw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJMJOVkmrvdtfniAnpQNH1JKICyiWojXF4O+iHkBlOiiFgrYymTNcPQ4xsr+kX/CHA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOMxsF1+mNFwLGgZErJrYiRyvvmDnqKxW3BLqHnIgPeUXFsTOPfW7cGsgwGsGQowMA==");

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "Description",
                value: "Kho nhập hàng lớn và lưu trữ chính");

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"),
                columns: new[] { "Description", "Name" },
                values: new object[] { "Kho bán lẻ & vật tư tại quầy", "Kho Quầy" });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceMaterials_MaintenanceLogId",
                table: "MaintenanceMaterials",
                column: "MaintenanceLogId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceMaterials_ProductId",
                table: "MaintenanceMaterials",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceMaterials");

            migrationBuilder.DropColumn(
                name: "AfterQuantity",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "BeforeQuantity",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "PerformedBy",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "AfterQuantity",
                table: "EquipmentTransactions");

            migrationBuilder.DropColumn(
                name: "BeforeQuantity",
                table: "EquipmentTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EquipmentTransactions");

            migrationBuilder.DropColumn(
                name: "AccumulatedDepreciation",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "IsFullyDepreciated",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "MonthlyDepreciationAmount",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "RemainingValue",
                table: "Equipments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL2wvjjkHqax12R0IeO+ODguHwmli6yrLHVA8J5m0CTDh62GaVnX4AQ+x6vZkHoIKQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDHh7OV6lje7fBgaONQUYkXCpquRud6tPk5tWUC5M5ECtPS5lonuv68WBkVpp09ZaQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM9tZAp/3jtyHi4NU2+/KgMF/58Lh7T9ouqV5OTPzcW08vJMcHzeBlbpMjiSQD6nYw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE40F2WdhaXvs8Ety3v5yZT9TFxlTki2Jj5w3TRjGDtUbXG48GCyYk7sTYVOo4YajA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECAqoY8uA+czJoMVALmMSoK0+qgOnK0kybFPALlaKbe3UfiyvA/YKAnwnvJzCmfq9w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPuOhyq8sS7QOf5JWNv81Hu/vUF5zj2EiZjp4DediwXtI5B0dJ4/R7xMlg3iXdmFjw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEELveAsQ3SY71Q/n1j8zsJsecQmn2VKh1VXCwfgoS6B47dZm1neH7jLEztv9lJfX+w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHI50zn4zIL6NRibpYN/CuMZbWkiER7zgY7M0Mk6y2Y1t0qY1xKBT75rDFw3zlsvoQ==");

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "Description",
                value: "Kho lưu trữ chính của phòng gym");

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"),
                columns: new[] { "Description", "Name" },
                values: new object[] { "Kho bán lẻ tại quầy tiếp khách", "Quầy Lễ Tân (Counter)" });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "IsDeleted", "Location", "Name", "UpdatedAt" },
                values: new object[] { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kho vật tư vận hành & vệ sinh", true, false, "Phòng kho tầng 1", "Kho Vật Tư (Supplies)", null });
        }
    }
}
