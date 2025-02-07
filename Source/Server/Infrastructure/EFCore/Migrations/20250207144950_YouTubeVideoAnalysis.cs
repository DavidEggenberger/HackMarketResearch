using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class YouTubeVideoAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "MarketResearches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "YouTubeVideoAnalysis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketResearchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YouTubeVideoAnalysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YouTubeVideoAnalysis_MarketResearches_MarketResearchId",
                        column: x => x.MarketResearchId,
                        principalTable: "MarketResearches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VideoComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YouTubeVideoAnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoComment_YouTubeVideoAnalysis_YouTubeVideoAnalysisId",
                        column: x => x.YouTubeVideoAnalysisId,
                        principalTable: "YouTubeVideoAnalysis",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoComment_YouTubeVideoAnalysisId",
                table: "VideoComment",
                column: "YouTubeVideoAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_YouTubeVideoAnalysis_MarketResearchId",
                table: "YouTubeVideoAnalysis",
                column: "MarketResearchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoComment");

            migrationBuilder.DropTable(
                name: "YouTubeVideoAnalysis");

            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "MarketResearches");
        }
    }
}
