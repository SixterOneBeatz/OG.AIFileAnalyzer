using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OG.AIFileAnalyzer.Persistence.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FileEntitiesChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "FileAnalyses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "FileAnalyses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
