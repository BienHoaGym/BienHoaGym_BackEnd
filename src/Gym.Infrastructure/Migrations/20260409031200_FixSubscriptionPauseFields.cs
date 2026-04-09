using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    public partial class FixSubscriptionPauseFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Chỉ thêm các cột còn thiếu cho PostgreSQL
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPausedAt",
                table: "MemberSubscriptions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AutoPauseExtensionDays",
                table: "MemberSubscriptions",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPausedAt",
                table: "MemberSubscriptions");

            migrationBuilder.DropColumn(
                name: "AutoPauseExtensionDays",
                table: "MemberSubscriptions");
        }
    }
}
