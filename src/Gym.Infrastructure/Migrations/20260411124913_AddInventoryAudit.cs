using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MembershipPackages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "StockAudits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuditDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PerformedBy = table.Column<string>(type: "TEXT", nullable: true),
                    ApprovedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAudits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockAudits_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockAuditDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StockAuditId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SystemQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ActualQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAuditDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockAuditDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockAuditDetails_StockAudits_StockAuditId",
                        column: x => x.StockAuditId,
                        principalTable: "StockAudits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "👑 Quản trị viên - TOÀN QUYỀN HỆ THỐNG", "[\"*\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "🏢 Quản lý - Điều hành vận hành chung");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "🏋️ Huấn luyện viên (PT) - Chuyên môn", "[\"member.read\",\"checkin.read\",\"class.read\",\"class.manage\",\"equipment.read\",\"equipment.report\",\"inventory.consume\",\"product.read\",\"trainer.read\",\"provider.read\",\"report.read\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "👩💼 Lễ tân - Vận hành và Check-in hàng ngày", "[\"member.read\",\"member.create\",\"member.update\",\"checkin.create\",\"checkin.read\",\"package.read\",\"inventory.read\",\"inventory.consume\",\"report.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"payment.read\",\"payment.create\",\"billing.read\",\"billing.create\",\"product.read\",\"class.read\",\"trainer.read\",\"provider.read\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "👤 Hội viên - Tự phục vụ");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "💰 Thủ quỹ/Kế toán - Quản lý tài chính", "[\"billing.read\",\"billing.create\",\"payment.read\",\"payment.create\",\"report.read\",\"package.read\",\"member.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"checkin.read\",\"product.read\",\"class.read\",\"trainer.read\",\"provider.read\"]" });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 49, 9, 497, DateTimeKind.Utc).AddTicks(7745));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 49, 9, 497, DateTimeKind.Utc).AddTicks(7752));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 49, 9, 497, DateTimeKind.Utc).AddTicks(7754));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 49, 9, 497, DateTimeKind.Utc).AddTicks(7755));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 49, 9, 497, DateTimeKind.Utc).AddTicks(7756));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 49, 9, 497, DateTimeKind.Utc).AddTicks(7759));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 49, 9, 497, DateTimeKind.Utc).AddTicks(7760));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 49, 9, 497, DateTimeKind.Utc).AddTicks(7761));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 49, 9, 497, DateTimeKind.Utc).AddTicks(7758));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 11, 12, 49, 9, 497, DateTimeKind.Utc).AddTicks(7757));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGibpqmv8S+qNSq5G1xPEdgTb3A0nstYGTSqWpj3hWMsk04mDFitpAsuryCktaF4Hw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOgfBTl1cd8eyASKN/muJ1hsa0Vik+EoXA8HvXwKe3EbOdsG73kil0xU3wEu/4BUrA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGTiUOWBf+GG37GP+DS+hIjA6Vs5N+joJXo3aniXwtiE9+i1CPy1pU6oRleokWQwVw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEP4SMdMaetH8Js9G+Gpj3qBsK4dIxZsWYVUOeuFd8u9RlOhtdkahfF5haHNdd45Kcg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENzBZOQ1K3IMxJmwYC033s2I+hKFi6SQbEo4IkBozxJKnyHsVRno3MgFd8RUuxIRww==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJaK2/QqrCeU0MPacGML3GCEN47kbykItZFPaBju1nsY/B3MFUMkCh71P5DAF9og5Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEV5k2RinkhPaF38r6rduTtj7hNl7awOPmbIavZAUfwC4iwwKdEr0fXBOx7as5LlLw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIdTzgTIbM9YZwp9M1IVs0XmYsoAXXJA06VimsyA+4RoH3TDjTey+pqSVUHHUJN6DA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJYQ9i3Ock0UxwllwOKz5EoY+es5C/Zqj/m7kyzRpB+FjYxaiCqtOxSRrkLHYvkcLQ==");

            migrationBuilder.CreateIndex(
                name: "IX_StockAuditDetails_ProductId",
                table: "StockAuditDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAuditDetails_StockAuditId",
                table: "StockAuditDetails",
                column: "StockAuditId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAudits_WarehouseId",
                table: "StockAudits",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockAuditDetails");

            migrationBuilder.DropTable(
                name: "StockAudits");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MembershipPackages",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "👑 Admin/Manager (Quản lý chủ gym) - TOÀN QUYỀN", "\"*\"" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Tiểu quản lý vận hành");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "🏋️ Trainer - Chuyên môn", "[\"member.read\",\"class.manage\",\"equipment.read\",\"equipment.report\",\"inventory.consume\",\"product.read\",\"trainer.read\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "👩💼 Receptionist - Vận hành hàng ngày", "[\"member.read\",\"member.create\",\"member.update\",\"checkin.create\",\"checkin.read\",\"package.read\",\"inventory.read\",\"inventory.consume\",\"report.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"payment.read\",\"payment.create\",\"billing.read\",\"billing.create\",\"product.read\",\"class.read\",\"trainer.read\"]" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "👤 Member - Tự phục vụ");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Permissions" },
                values: new object[] { "💰 Accountant - Quản lý tài chính", "[\"billing.read\",\"billing.create\",\"payment.read\",\"payment.create\",\"report.read\",\"package.read\",\"member.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"checkin.read\",\"product.read\",\"class.read\",\"trainer.read\"]" });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, new Guid("11111111-1111-1111-1111-111111111111") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 10, 13, 47, 45, 275, DateTimeKind.Utc).AddTicks(1674));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("22222222-2222-2222-2222-222222222222") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 10, 13, 47, 45, 275, DateTimeKind.Utc).AddTicks(1682));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("33333333-3333-3333-3333-333333333333") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 10, 13, 47, 45, 275, DateTimeKind.Utc).AddTicks(1684));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("44444444-4444-4444-4444-444444444444") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 10, 13, 47, 45, 275, DateTimeKind.Utc).AddTicks(1685));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("55555555-5555-5555-5555-555555555555") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 10, 13, 47, 45, 275, DateTimeKind.Utc).AddTicks(1686));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("66666666-6666-6666-6666-666666666666") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 10, 13, 47, 45, 275, DateTimeKind.Utc).AddTicks(1689));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("77777777-7777-7777-7777-777777777777") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 10, 13, 47, 45, 275, DateTimeKind.Utc).AddTicks(1690));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 5, new Guid("88888888-8888-8888-8888-888888888888") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 10, 13, 47, 45, 275, DateTimeKind.Utc).AddTicks(1691));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 10, 13, 47, 45, 275, DateTimeKind.Utc).AddTicks(1688));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("99999999-9999-9999-9999-999999999999") },
                column: "AssignedAt",
                value: new DateTime(2026, 4, 10, 13, 47, 45, 275, DateTimeKind.Utc).AddTicks(1687));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI2cvAUkwNBWbyO7LQ0LCWi31ueocjms2ReHdKi3bvG229bjtM4x5ymgbeboBkxO3g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBC+sI3SbeU1TZuu37S11A4Bj61R4avDN1o6swg9InkRH3HdVjitY/e4J4FuYUGrfw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJhTG9+mfg6lHyC0NUhzwY5Ry/YQRdw4WBxfymi9U51Av6ETvQ++5teuR8eIBJhahQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKElaQmhdQ6TR0OgFn54llOBYPR02d8rrehUrwTHQqDpbVVmA4YHIb1lCZX1EuWyPA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDojhhWgBMktK3kNmrTvY4hEflcIhJITiiMLPwjMif/Ivu6qGfhv0LGEOSolps43mQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKzAEaVVAVqUGv+2LAGy6SSoVv8CwafliTWApSLqTFwB6uSBiluSYtKNjY8FLRiNRA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEECfvNQym4j34miP6Ei5X5umFbB1MONeUNYVl0E6CBPx8FHC6tQNe2bGWuQDoXF1lw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJvURQswafhNW+A/eILorpwPliiqGdHFLhRzsGU9v4ZUeBH2Hpia/+0KOqKnFn3AvA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDG6xTd5eOzkKlOf0D8OFAyAh01QnV55B4zdR/ht8rxtasjFrMgGloRO0VDv6dxGcA==");
        }
    }
}
