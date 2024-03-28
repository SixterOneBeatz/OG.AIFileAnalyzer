using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OG.AIFileAnalyzer.Persistence.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AnalysisChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAnalyses_Files_FileEntityId",
                table: "FileAnalyses");

            migrationBuilder.DropIndex(
                name: "IX_FileAnalyses_FileEntityId",
                table: "FileAnalyses");

            migrationBuilder.DropColumn(
                name: "FileEntityId",
                table: "FileAnalyses");

            migrationBuilder.AddColumn<int>(
                name: "DocumentType",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "FileAnalyses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FileAnalyses_FileId",
                table: "FileAnalyses",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAnalyses_Files_FileId",
                table: "FileAnalyses",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAnalyses_Files_FileId",
                table: "FileAnalyses");

            migrationBuilder.DropIndex(
                name: "IX_FileAnalyses_FileId",
                table: "FileAnalyses");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "FileAnalyses");

            migrationBuilder.AddColumn<int>(
                name: "FileEntityId",
                table: "FileAnalyses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileAnalyses_FileEntityId",
                table: "FileAnalyses",
                column: "FileEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAnalyses_Files_FileEntityId",
                table: "FileAnalyses",
                column: "FileEntityId",
                principalTable: "Files",
                principalColumn: "Id");
        }
    }
}
