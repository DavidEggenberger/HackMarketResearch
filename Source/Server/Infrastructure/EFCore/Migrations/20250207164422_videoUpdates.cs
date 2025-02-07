using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class videoUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoComment_YouTubeVideoAnalysis_YouTubeVideoAnalysisId",
                table: "VideoComment");

            migrationBuilder.DropForeignKey(
                name: "FK_YouTubeVideoAnalysis_MarketResearches_MarketResearchId",
                table: "YouTubeVideoAnalysis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_YouTubeVideoAnalysis",
                table: "YouTubeVideoAnalysis");

            migrationBuilder.RenameTable(
                name: "YouTubeVideoAnalysis",
                newName: "YouTubeVideoAnalyses");

            migrationBuilder.RenameIndex(
                name: "IX_YouTubeVideoAnalysis_MarketResearchId",
                table: "YouTubeVideoAnalyses",
                newName: "IX_YouTubeVideoAnalyses_MarketResearchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_YouTubeVideoAnalyses",
                table: "YouTubeVideoAnalyses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoComment_YouTubeVideoAnalyses_YouTubeVideoAnalysisId",
                table: "VideoComment",
                column: "YouTubeVideoAnalysisId",
                principalTable: "YouTubeVideoAnalyses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_YouTubeVideoAnalyses_MarketResearches_MarketResearchId",
                table: "YouTubeVideoAnalyses",
                column: "MarketResearchId",
                principalTable: "MarketResearches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoComment_YouTubeVideoAnalyses_YouTubeVideoAnalysisId",
                table: "VideoComment");

            migrationBuilder.DropForeignKey(
                name: "FK_YouTubeVideoAnalyses_MarketResearches_MarketResearchId",
                table: "YouTubeVideoAnalyses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_YouTubeVideoAnalyses",
                table: "YouTubeVideoAnalyses");

            migrationBuilder.RenameTable(
                name: "YouTubeVideoAnalyses",
                newName: "YouTubeVideoAnalysis");

            migrationBuilder.RenameIndex(
                name: "IX_YouTubeVideoAnalyses_MarketResearchId",
                table: "YouTubeVideoAnalysis",
                newName: "IX_YouTubeVideoAnalysis_MarketResearchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_YouTubeVideoAnalysis",
                table: "YouTubeVideoAnalysis",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoComment_YouTubeVideoAnalysis_YouTubeVideoAnalysisId",
                table: "VideoComment",
                column: "YouTubeVideoAnalysisId",
                principalTable: "YouTubeVideoAnalysis",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_YouTubeVideoAnalysis_MarketResearches_MarketResearchId",
                table: "YouTubeVideoAnalysis",
                column: "MarketResearchId",
                principalTable: "MarketResearches",
                principalColumn: "Id");
        }
    }
}
