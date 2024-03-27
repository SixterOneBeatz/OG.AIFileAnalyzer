using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OG.AIFileAnalyzer.Persistence.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class LogChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "Logs");
        }
    }
}
