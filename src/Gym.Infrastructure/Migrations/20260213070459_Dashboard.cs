using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Dashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELmi40BRuMhtMrwiPb2SA3OK5LE8W5pxEYRlySb4aks3FnDT6pR6rLNOiD6WvkUApA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIL2saMjGHgKBKol0a2zmc9GK0IM15ahgzTRiWTi9j0m+FHwIaDwNfDS1dzcqe9yWg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEB3kLTfyzlpcm/I7r2QNs1O763HgqKAH2TB43qK9VIB/bETroc5L3ziGNQ8FCR8FA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEACe9K5dvMkXvSmlGM0cYOkU1VNyAUkZ95f8oexvVZegfmx5ASHpSTPW1MAbxPWbVQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFR0hIlN5AuK0ml6ZD5wgK2Ub/VWCQ4uW1P5pQvfSQ1no5Mj2unAO1GmUu8/gk1Bzw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC1h24VJSVlpd6hsbzSmfeg/og6e3TBjIpQef5bmHc26y1GocmaKCHB9NqyCD26Zrw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKpaBNpBoI8ATRBiv5Oi3YGhs0MQy17Itu6Bw+i6Iuhvh7s/N4FuXJye74V5PvSdlg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEK1pYiD+xCDi/Ek+C6JEP7J92qyS5Gkjt6Y6GWPDA176mWiGiMdW/0sKAdqXM+jsag==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHfWXyU6uWwVadi4T6ReS+A/vmwhKgzPJd8YpV+aggDy1qy1YIaR0T2RO47MGCTvmA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKKm+j5Nf5ANxzCqhDAov7WsTuAdZQyEUTZ4skYTe+D/33eoc+bEjTBq5CjufVaDQw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFEZlnu4vjmcKE8n+qmWHcuS5nomk7qm3ie5PA5//9Hk1sDEl1ckdKeEwmds7LAkkg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFvDOa/gWzRHNEMRxQfbyEkkYoH4ACf+VE2msJUwVbhzopOr/z5QtMqzCJgWcDMyqg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEwTNWgV1DHgqWT06Lid9xxbihZ7eSiG310YqahSYdodG/xuJr/38+YnTk6jF26Acw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN8DAiv8ruDWJvQ67OXf5fsONBX1DHMu4+qALVxE8XhK26K+qd1qhbjQUzKzWTnGDQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEED9fbsv8ynj2PQ6PtzZoLNyPkINoi+TJTbhqzSN7bIAqcRsGcyrQNg3PbvmRz8hgw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBkIow4as09pKB/NjDUj5ACl49ZjytnokL0JctDQvFb0WV0Z6qy21I5GYBYvBgtYkQ==");
        }
    }
}
