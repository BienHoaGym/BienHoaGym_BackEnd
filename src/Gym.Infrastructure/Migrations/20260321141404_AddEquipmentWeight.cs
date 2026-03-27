using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipmentWeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Equipments",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAiPRewCheaWJ28O+VoFTuxkyd+/RBTNSyc+fJUwJ7ybX4HTcnMlishvtvtm3PpRLw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBnraX7+c0FOE1cSLk9uGh6RlSY07iKfBnHqPkQLxfho1Ej1zRbYEL+zsa+exPp8/g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOxdqFx1070h3dUhgK0gRPluD7+77Y8R1WcHJ37aIapQCRJJHE/vPh/3TxtfOsesUQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELDj0c9+qyoCkSsGbo94LdTJiWWT1OZgG9nTv2895za0ry/MgqnGMmAkGC/VsnlMxw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOdcVz4SKI1pGRWB5uLlei7AdTBBUtOkAKJw+p1BvmKgVUStCtxd8i6jcR2nh1CNnA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMHdAX2qmn/geIemc0l8AVsXo/QfoNifRj2Ekc9t4D+h/QMdPipM7wU6u4dM4sQ1gA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKDKka5kuaQAQfeHqRb2WPI5oz/zUSpMfSpQkcYdv6hc8xcDTwjaT6MXLGL9958WvQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDTEeJiN2JYMgREI1qdnqRANJO+AmGsU6iUcLrlTd2gHsUA6S2EdM/dHPnmjvz+cYg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Equipments");

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
        }
    }
}
