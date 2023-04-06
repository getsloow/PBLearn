using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL.Migrations
{
    /// <inheritdoc />
    public partial class TurnInTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TurnedInAt",
                table: "Assignments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurnedInAt",
                table: "Assignments");
        }
    }
}
