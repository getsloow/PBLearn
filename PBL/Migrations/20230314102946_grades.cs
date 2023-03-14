using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL.Migrations
{
    /// <inheritdoc />
    public partial class grades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Grade",
                table: "Projects",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Grade",
                table: "Assignments",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Assignments");
        }
    }
}
