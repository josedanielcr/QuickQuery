using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickqueryDataGatewayAPI.Migrations
{
    /// <inheritdoc />
    public partial class country3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryPriority",
                table: "Countries",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryPriority",
                table: "Countries");
        }
    }
}
