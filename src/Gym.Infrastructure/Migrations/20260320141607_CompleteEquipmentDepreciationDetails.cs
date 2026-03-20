using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CompleteEquipmentDepreciationDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Depreciations",
                newName: "Amount");

            migrationBuilder.AddColumn<DateTime>(
                name: "DepreciationStartDate",
                table: "Equipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalvageValue",
                table: "Equipments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "UsefulLifeMonths",
                table: "Equipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Depreciations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeriodMonth",
                table: "Depreciations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PeriodYear",
                table: "Depreciations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingValue",
                table: "Depreciations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO3pqjT95KxYtcO+EgQkVnJctCCkD78P11ePimx8e6p3v/9InEkN9Z0l7HPXEXTgQA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAlsqAG7hairCQT5i0hX9SrzexnHwvMXHjnOzeWAMKIeQBUvqw6FP8k2/NAxQd2vlg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF8rVEI9GdY2Wo/PmW/YBAHU9wuWd/fsiGN09xAtIhmJZLV9qbTKLg9Pfi2VBtDmuA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBhRGXOjQPnyvjlapF2Pi8RgtnefZnoBYNA1iiipM6vftmcDDxbqdeWfoBgxAamxlQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELriaiBRKhGS74JjqmFOZrwf5d5vYQuQYh675DNt7CTI3P2ett3o2kgbDKV4/rPH9g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFox9DKxEp65471//B6cJtFazroQZV+xQuRxnMJs/C1DFCRZYZtZll86t+RsVaW8cg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKFYY2RVfpj4TxWF6OuOkQxHGMJywmMgbOxy0BCaIixbPlanrbDILyFLPugW14g3Jg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMgwo4/YV+5bOaD1tp5qEpbRfTkAWA5jGgecqSZXoTc6Pc9RfI3GjkPXJKRmcDfPlQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepreciationStartDate",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "SalvageValue",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "UsefulLifeMonths",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Depreciations");

            migrationBuilder.DropColumn(
                name: "PeriodMonth",
                table: "Depreciations");

            migrationBuilder.DropColumn(
                name: "PeriodYear",
                table: "Depreciations");

            migrationBuilder.DropColumn(
                name: "RemainingValue",
                table: "Depreciations");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Depreciations",
                newName: "Value");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENzARoStWlBQ9YMWcFMnf/gnMY4HugHW9/zoQpbC3lRr/N8pZcrT6H0J0Zr1iKTmLw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKOXqHHPQsXSo9dPO4ohwBmywDYbRSoKmKSVbL+NlTNCvltjwDvMhtRjtivJSlwyNQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH5BICOi9Cy1YGR4p8ECfDNeD6j6j/83aAcHJB7VOEIOidxnv6p3aS3/GSoP3sgZJg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBaA44d8Eb5cRGb2jqQlCWMW8qPBtNDCumZNTdo/1j5Qq7ws8WFil19L6rgEoIR3KQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF4MR0HyFXJQY9aLj7L3EbVS3+UvLxmyZt8maeY9z0qmWWiPJsQ29LVpbXpZ6m3ucw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIeqLMxQCApiGGrl8bTJDSfWzw+sHQhfbqLE2JSVT6CHOsiQ5YdvGI6snLVkhUn+LQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELmMsDmZhSGudT4Sq7rNJI6DHsfTCde7Q33GXD2L0fKVQ8dRj6WxduXEbQny5/xb7g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOks+Rk/eEdNb2Nspr0i/LdVVDfEsLhzPxIhNwTu4KXEtLk/Z7PoODhnaumIVZq71Q==");
        }
    }
}
