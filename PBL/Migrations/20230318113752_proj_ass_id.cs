using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL.Migrations
{
    /// <inheritdoc />
    public partial class proj_ass_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "Files",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_AssignmentId",
                table: "Files",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_ProjectId",
                table: "Files",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Assignments_AssignmentId",
                table: "Files",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Projects_ProjectId",
                table: "Files",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Assignments_AssignmentId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Projects_ProjectId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_AssignmentId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ProjectId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Files");
        }
    }
}
