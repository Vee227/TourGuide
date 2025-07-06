using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourGuide.Migrations
{
    /// <inheritdoc />
    public partial class AddMapImagePathToTour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mapImagePath",
                table: "Tours",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mapImagePath",
                table: "Tours");
        }
    }
}
