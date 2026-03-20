using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumns_Providers_Equipments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
