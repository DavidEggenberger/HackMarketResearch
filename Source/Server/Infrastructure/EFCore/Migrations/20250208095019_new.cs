using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PotentialMarket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketResearchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PotentialMarket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PotentialMarket_MarketResearches_MarketResearchId",
                        column: x => x.MarketResearchId,
                        principalTable: "MarketResearches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PotentialMarket_MarketResearchId",
                table: "PotentialMarket",
                column: "MarketResearchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PotentialMarket");
        }
    }
}
