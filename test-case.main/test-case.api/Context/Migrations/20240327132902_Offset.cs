using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test_case.api.Context.Migrations
{
    /// <inheritdoc />
    public partial class Offset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "Offset",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Offset",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
