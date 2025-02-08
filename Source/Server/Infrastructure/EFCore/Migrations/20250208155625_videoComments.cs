using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class videoComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoComment_YouTubeVideoAnalyses_YouTubeVideoAnalysisId",
                table: "VideoComment");

            migrationBuilder.AlterColumn<Guid>(
                name: "YouTubeVideoAnalysisId",
                table: "VideoComment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoComment_YouTubeVideoAnalyses_YouTubeVideoAnalysisId",
                table: "VideoComment",
                column: "YouTubeVideoAnalysisId",
                principalTable: "YouTubeVideoAnalyses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoComment_YouTubeVideoAnalyses_YouTubeVideoAnalysisId",
                table: "VideoComment");

            migrationBuilder.AlterColumn<Guid>(
                name: "YouTubeVideoAnalysisId",
                table: "VideoComment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoComment_YouTubeVideoAnalyses_YouTubeVideoAnalysisId",
                table: "VideoComment",
                column: "YouTubeVideoAnalysisId",
                principalTable: "YouTubeVideoAnalyses",
                principalColumn: "Id");
        }
    }
}
