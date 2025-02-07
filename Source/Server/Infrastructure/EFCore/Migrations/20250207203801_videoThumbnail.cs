using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class videoThumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "YouTubeVideoAnalyses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "YouTubeVideoAnalyses");
        }
    }
}
