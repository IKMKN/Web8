using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web8.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 25, 11, 52, 8, 734, DateTimeKind.Utc).AddTicks(3810));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 25, 11, 46, 7, 944, DateTimeKind.Utc).AddTicks(8774));
        }
    }
}
