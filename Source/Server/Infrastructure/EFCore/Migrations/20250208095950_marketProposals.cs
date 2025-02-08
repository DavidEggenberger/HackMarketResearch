using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class marketProposals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PotentialMarket");

            migrationBuilder.CreateTable(
                name: "MarketProposal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChatMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MarketResearchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketProposal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketProposal_ChatMessage_ChatMessageId",
                        column: x => x.ChatMessageId,
                        principalTable: "ChatMessage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MarketProposal_MarketResearches_MarketResearchId",
                        column: x => x.MarketResearchId,
                        principalTable: "MarketResearches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketProposal_ChatMessageId",
                table: "MarketProposal",
                column: "ChatMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketProposal_MarketResearchId",
                table: "MarketProposal",
                column: "MarketResearchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketProposal");

            migrationBuilder.CreateTable(
                name: "PotentialMarket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarketResearchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
    }
}
