using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace medgem_backend.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "HomePageContent",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 10, 28, 9, 51, 40, 25, DateTimeKind.Utc).AddTicks(2466));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 10, 28, 9, 51, 40, 25, DateTimeKind.Utc).AddTicks(9891));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "HomePageContent",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 10, 28, 9, 20, 14, 748, DateTimeKind.Utc).AddTicks(2297));

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 10, 28, 9, 20, 14, 749, DateTimeKind.Utc).AddTicks(2438));
        }
    }
}
