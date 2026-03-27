using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RBAC_Implementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentProviderHistories_Providers_NewProviderId",
                table: "EquipmentProviderHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentProviderHistories_Providers_OldProviderId",
                table: "EquipmentProviderHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Members_MemberId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "VATCode",
                table: "Providers",
                newName: "TaxCode");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Providers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Providers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OriginalPackageName",
                table: "MemberSubscriptions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "QRCode",
                table: "Members",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "Invoices",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ItemName",
                table: "InvoiceDetails",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EquipmentCategories",
                columns: new[] { "Id", "AvgMaintenanceCost", "Code", "CreatedAt", "Description", "Group", "IsDeleted", "Name", "StandardWarrantyMonths", "UpdatedAt" },
                values: new object[] { new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), 200000m, "", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Các loại máy chạy bộ, xe đạp, trượt tuyết", null, false, "Máy Cardio", null, null });

            migrationBuilder.InsertData(
                table: "Providers",
                columns: new[] { "Id", "Address", "BankAccountNumber", "BankName", "Code", "ContactPerson", "CreatedAt", "Email", "IsActive", "IsDeleted", "Name", "Note", "PhoneNumber", "SupplyType", "TaxCode", "UpdatedAt" },
                values: new object[] { new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), "123 Đường số 7, TP.HCM", null, null, null, "Nguyễn Văn Cung", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "contact@gymglobal.com", true, false, "Công ty Thiết bị Gym Toàn Cầu", null, "02838445566", null, "0314567890", null });

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

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Permissions", "RoleName" },
                values: new object[] { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hội viên tự phục vụ", "[\"checkin.self\", \"member.profile.read\", \"subscription.read\", \"class.enroll\"]", "Member" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "AssignedAt" },
                values: new object[,]
                {
                    { 1, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 3, 22, 8, 43, 1, 651, DateTimeKind.Utc).AddTicks(3504) },
                    { 2, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8034) },
                    { 3, new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8062) },
                    { 3, new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8068) },
                    { 4, new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8074) }
                });

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
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "AQAAAAIAAYagAAAAEC1a+Pn1Ii+PE19ofALeUCtQXWx7RED+DVEgFfmL929ag1jo/iLchssIX3kz/8Gjyw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "AQAAAAIAAYagAAAAEOa4HfRKa+2HRh3+OT7apWLu9nYcuUmjw1jafbV9tn5uT75wPyYIk0u3wtAnYO+71w==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "CreatedAt", "Email", "FullName", "PasswordHash" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "levanc_member@gmail.com", "Lê Văn C (Member)", "AQAAAAIAAYagAAAAEKkNajLEIctqwbAP04xPfpmFNRXvX6NEYN8DiWmvgj/x9dVh1eKJrvDTJIKDPPk6Sw==" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "PasswordHash", "PhoneNumber", "UpdatedAt", "Username" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "levanc@gym.com", "Lê Văn C", true, null, "AQAAAAIAAYagAAAAEDzXsvG/6MmwSDiXLTnkX4g/EHhUm4aavZQVZZheJbenCpLdkdyZXMSZsELzRwfL3g==", "0909999999", null, "levanc" });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "Id", "AccumulatedDepreciation", "CategoryId", "CreatedAt", "DepreciationStartDate", "Description", "EquipmentCode", "IsDeleted", "IsFullyDepreciated", "LastMaintenanceDate", "Location", "MaintenanceIntervalDays", "MonthlyDepreciationAmount", "Name", "NextMaintenanceDate", "Priority", "ProviderId", "PurchaseDate", "PurchasePrice", "Quantity", "RemainingValue", "SalvageValue", "SerialNumber", "Status", "UpdatedAt", "UsefulLifeMonths", "WarrantyExpiryDate", "Weight" },
                values: new object[] { new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), 0m, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "EQP-RUN-001", false, false, null, "Khu Cardio Tầng 1", 90, 333333m, "Máy chạy bộ Matrix T7", null, 3, new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 25000000m, 5, 25000000m, 5000000m, "MTX7-2024-X1", 1, null, 60, null, null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Barcode", "Category", "CostPrice", "CreatedAt", "Description", "DurationDays", "ExpirationDate", "ImageUrl", "IsActive", "IsDeleted", "MaxStockThreshold", "MinStockThreshold", "Name", "Price", "ProviderId", "SKU", "SessionCount", "StockQuantity", "TrackInventory", "Type", "Unit", "UpdatedAt" },
                values: new object[] { new Guid("91919191-9191-9191-9191-919191919191"), null, "Đồ uống", 5000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nước khoáng thiên nhiên", null, null, null, true, false, 200, 20, "Nước suối Lavie 500ml", 10000m, new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), "WTR-001", null, 100, true, 3, "Chai", null });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "AssignedAt" },
                values: new object[,]
                {
                    { 5, new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8154) },
                    { 5, new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8159) },
                    { 5, new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8164) },
                    { 3, new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8149) },
                    { 4, new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 3, 22, 8, 43, 1, 652, DateTimeKind.Utc).AddTicks(8080) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber",
                table: "Invoices",
                column: "InvoiceNumber",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentProviderHistories_Providers_NewProviderId",
                table: "EquipmentProviderHistories",
                column: "NewProviderId",
                principalTable: "Providers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentProviderHistories_Providers_OldProviderId",
                table: "EquipmentProviderHistories",
                column: "OldProviderId",
                principalTable: "Providers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Members_MemberId",
                table: "Invoices",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentProviderHistories_Providers_NewProviderId",
                table: "EquipmentProviderHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentProviderHistories_Providers_OldProviderId",
                table: "EquipmentProviderHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Members_MemberId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Products_SKU",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_InvoiceNumber",
                table: "Invoices");

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "Id",
                keyValue: new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("91919191-9191-9191-9191-919191919191"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "EquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"));

            migrationBuilder.DeleteData(
                table: "Providers",
                keyColumn: "Id",
                keyValue: new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"));

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Providers");

            migrationBuilder.RenameColumn(
                name: "TaxCode",
                table: "Providers",
                newName: "VATCode");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Providers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalPackageName",
                table: "MemberSubscriptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "QRCode",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ItemName",
                table: "InvoiceDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Full system access");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "Manage gym operations", "[\"members.*\", \"packages.*\", \"subscriptions.*\", \"payments.*\", \"checkins.*\", \"reports.*\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "Manage classes and view members", "[\"classes.*\", \"members.view\", \"checkins.view\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "Check-in and basic operations", "[\"checkins.*\", \"members.view\", \"subscriptions.view\"]" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "PasswordHash", "RoleId" },
                values: new object[] { "AQAAAAIAAYagAAAAEAiPRewCheaWJ28O+VoFTuxkyd+/RBTNSyc+fJUwJ7ybX4HTcnMlishvtvtm3PpRLw==", 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "PasswordHash", "RoleId" },
                values: new object[] { "AQAAAAIAAYagAAAAEBnraX7+c0FOE1cSLk9uGh6RlSY07iKfBnHqPkQLxfho1Ej1zRbYEL+zsa+exPp8/g==", 2 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "PasswordHash", "RoleId" },
                values: new object[] { "AQAAAAIAAYagAAAAEOxdqFx1070h3dUhgK0gRPluD7+77Y8R1WcHJ37aIapQCRJJHE/vPh/3TxtfOsesUQ==", 3 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "PasswordHash", "RoleId" },
                values: new object[] { "AQAAAAIAAYagAAAAELDj0c9+qyoCkSsGbo94LdTJiWWT1OZgG9nTv2895za0ry/MgqnGMmAkGC/VsnlMxw==", 3 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "PasswordHash", "RoleId" },
                values: new object[] { "AQAAAAIAAYagAAAAEOdcVz4SKI1pGRWB5uLlei7AdTBBUtOkAKJw+p1BvmKgVUStCtxd8i6jcR2nh1CNnA==", 4 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "CreatedAt", "PasswordHash", "RoleId" },
                values: new object[] { new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "AQAAAAIAAYagAAAAEMHdAX2qmn/geIemc0l8AVsXo/QfoNifRj2Ekc9t4D+h/QMdPipM7wU6u4dM4sQ1gA==", 4 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAt", "PasswordHash", "RoleId" },
                values: new object[] { new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), "AQAAAAIAAYagAAAAEKDKka5kuaQAQfeHqRb2WPI5oz/zUSpMfSpQkcYdv6hc8xcDTwjaT6MXLGL9958WvQ==", 4 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "CreatedAt", "Email", "FullName", "PasswordHash", "RoleId" },
                values: new object[] { new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "levanc@gmail.com", "Lê Văn C", "AQAAAAIAAYagAAAAEDTEeJiN2JYMgREI1qdnqRANJO+AmGsU6iUcLrlTd2gHsUA6S2EdM/dHPnmjvz+cYg==", 4 });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentProviderHistories_Providers_NewProviderId",
                table: "EquipmentProviderHistories",
                column: "NewProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentProviderHistories_Providers_OldProviderId",
                table: "EquipmentProviderHistories",
                column: "OldProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Members_MemberId",
                table: "Invoices",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
