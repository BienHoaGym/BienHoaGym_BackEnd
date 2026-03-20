using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
