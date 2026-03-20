using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEquipmentAndProviderFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "Providers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Providers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquipmentCode",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EquipmentProviderHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NewProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentProviderHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentProviderHistories_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentProviderHistories_Providers_NewProviderId",
                        column: x => x.NewProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentProviderHistories_Providers_OldProviderId",
                        column: x => x.OldProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncidentLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolutionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentLogs_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKHP6gVUOPXcUlj7yDDnUV0VXkLH/Ec5nFpExFloBv76Sxa5piNZMCOkM+Im/SB2EQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEB/+Wh1yX8QF1Lmx7OnuUd0MX07j0Mr+umDHGMEc166pNefq7WVqD223NN9esmXBrA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHIfoMEjrqVAIfo41nK04/3Tv7dvuiHgQUX6eY4AvNKMcuUsF+UsEX3z+8eorAdJBQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH5HT/wd52EOksPhv6Eu0x5N7nUhDebfQHEEPnVyJChp8obwlFpKza/I1XwWN4e0UA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENFQYhLn9/yDSoI228cuDsRaW9q3XequcB5rC64jHM/fwLw4GzAV+snU44C4JuxeKA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ5gcFTsWrSh8Zjmy6DGL4Zv71wwkvSr2ZW5YujbAQ5X7xT133r3EJm4FhrPpG8mig==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEB86FkTjAUJJlv/2a6uF8TjvsPsmVDfxAwqrqPylMWiRp4IM3rwmdTj7a3mizj5kBA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIq/kfrte8c9rFKIDAxuht1Stc1LqGjTUBnOA/PUuNzFl2BCqZl1c5qc8rrEy90wGA==");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviderHistories_EquipmentId",
                table: "EquipmentProviderHistories",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviderHistories_NewProviderId",
                table: "EquipmentProviderHistories",
                column: "NewProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviderHistories_OldProviderId",
                table: "EquipmentProviderHistories",
                column: "OldProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentLogs_EquipmentId",
                table: "IncidentLogs",
                column: "EquipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentProviderHistories");

            migrationBuilder.DropTable(
                name: "IncidentLogs");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "EquipmentCode",
                table: "Equipments");

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
        }
    }
}
