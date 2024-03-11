using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSale.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Routes",
                newName: "CountryOfDestination");

            migrationBuilder.AddColumn<string>(
                name: "CountryOfDeparture",
                table: "Routes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryOfDeparture",
                table: "Routes");

            migrationBuilder.RenameColumn(
                name: "CountryOfDestination",
                table: "Routes",
                newName: "Country");
        }
    }
}
