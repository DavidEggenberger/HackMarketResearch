using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class videoName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "YouTubeVideoAnalyses",
                newName: "VideoName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoName",
                table: "YouTubeVideoAnalyses",
                newName: "Description");
        }
    }
}
