using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExpandInventoryModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FromWarehouseId",
                table: "StockTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToWarehouseId",
                table: "StockTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "StockTransactions",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MaxStockThreshold",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "Inventories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMqVKfIIf8gj/tlT/ppEdSFBoJTTfnIvvI+UZoR2ZjrFvxEflXPTyt4kIvrsDlPtOw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENzQgb6OjyS639htIHmW+0qjQd6ly2QmxyQOfhwai577vHOjlaj9NBKzzbomVb+CWw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG7K0fkGOV/Bsq6fa4Wrx5iiqyzKSYPgDbiVg5raXpBG1OuT9e9DyNijWBkp5BB1+w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGIk+b+RacP4u2TNxWKp2rDakmJB31ugFYd8FNOd84Usr/bvqf7DWp2CTa8aMHF5gQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECHnAnj0fk4cyv9SDSSe3smrqcMwdFVdWxNz6MJdPDVrcOvLjBIt+QBZnJqq+8ZZYg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGM+5zLo9mxTOyS0MdmDg03YngaYCbZB84dYgwUrb/PBHrnSFosmIjaw/4j+ZgrqMg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHp2LMdOOoaGQbmKjBpYvtSc9NiCPwMx6IvPhpz7YQpQTDgbm7lbsKvuhIE4ys1ITQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG1R3HsRY4kvwk/YGSgvo3Unmo/Yv+Ifx7wf77sjgOzCUEww1yobXnUPJWRd1Obvww==");

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "IsDeleted", "Location", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kho lưu trữ chính của phòng gym", true, false, "Tầng hầm", "Kho Tổng (Main)", null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kho bán lẻ tại quầy tiếp khách", true, false, "Sảnh chính", "Quầy Lễ Tân (Counter)", null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kho vật tư vận hành & vệ sinh", true, false, "Phòng kho tầng 1", "Kho Vật Tư (Supplies)", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_FromWarehouseId",
                table: "StockTransactions",
                column: "FromWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ToWarehouseId",
                table: "StockTransactions",
                column: "ToWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_WarehouseId",
                table: "Inventories",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Warehouses_WarehouseId",
                table: "Inventories",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Warehouses_FromWarehouseId",
                table: "StockTransactions",
                column: "FromWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Warehouses_ToWarehouseId",
                table: "StockTransactions",
                column: "ToWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Warehouses_WarehouseId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Warehouses_FromWarehouseId",
                table: "StockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Warehouses_ToWarehouseId",
                table: "StockTransactions");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_FromWarehouseId",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_ToWarehouseId",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_WarehouseId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "FromWarehouseId",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "ToWarehouseId",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "MaxStockThreshold",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Inventories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJJX4eCcy37xmbLfqP6J2nKnD2P+HsShv5BN0qUH38waSPWSKdLtanzXFPem4YpA6Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMam6QNHoHrtRVCwlmaohFLNGH9+r/l+fkBl3IXe3qW2R0/sny45Jvs/ggO8tMdxEA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKmYaF7cq9tMRTKdtXWihrUS9/evRo5qGXkzPbC0FepltyCZ3dbvqYs8tsiFrx1b1A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFwUZq1IeLOaJOXljIFJDIAP9WULe8/1liyPKZL1DAfbKVey+hSfJNxG59jkBExaxQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMiu1x74kGgmkXfFE1RrO89ZVamk+ZhQgoWtqlnsQ8YMOb35tdFwlqrMwht7jWUPqQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOhvAp455sqOLdMVhP9nEB08HKdJ7BU1NOmK86BebbUKzf4Yx/Z+cncou+RsYrrqkw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC58v2ZIQkOUNyTvRxo0AGEsy8HZPWrdZ0ywOv/Q7fSqrXzRidEw/06gL+DBR2D8Sg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKCgatpTmIhTcIap7+rm2gUsVv1txr7ljUJ6pOnTa1Tvq2yWyzEZ/IC4/hWM0Jlr9Q==");
        }
    }
}
