using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourGuide.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTourNameFromTourLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TourName",
                table: "TourLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TourName",
                table: "TourLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
