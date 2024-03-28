using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OG.AIFileAnalyzer.Persistence.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FileEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SHA256 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileEntityId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileAnalyses_Files_FileEntityId",
                        column: x => x.FileEntityId,
                        principalTable: "Files",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileAnalyses_FileEntityId",
                table: "FileAnalyses",
                column: "FileEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_SHA256",
                table: "Files",
                column: "SHA256",
                unique: true,
                filter: "[SHA256] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileAnalyses");

            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
