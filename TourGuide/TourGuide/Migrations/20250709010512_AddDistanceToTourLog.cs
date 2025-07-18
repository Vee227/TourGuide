﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourGuide.Migrations
{
    /// <inheritdoc />
    public partial class AddDistanceToTourLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Distance",
                table: "TourLogs",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "TourLogs");
        }
    }
}
