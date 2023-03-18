using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL.Migrations
{
    /// <inheritdoc />
    public partial class upload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Files",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Files",
                newName: "Location");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Files",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Files",
                newName: "FileName");
        }
    }
}
