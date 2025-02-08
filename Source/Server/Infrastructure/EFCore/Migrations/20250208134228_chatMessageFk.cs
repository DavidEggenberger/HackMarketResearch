using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class chatMessageFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketProposal_ChatMessage_ChatMessageId",
                table: "MarketProposal");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChatMessageId",
                table: "MarketProposal",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MarketProposal_ChatMessage_ChatMessageId",
                table: "MarketProposal",
                column: "ChatMessageId",
                principalTable: "ChatMessage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketProposal_ChatMessage_ChatMessageId",
                table: "MarketProposal");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChatMessageId",
                table: "MarketProposal",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_MarketProposal_ChatMessage_ChatMessageId",
                table: "MarketProposal",
                column: "ChatMessageId",
                principalTable: "ChatMessage",
                principalColumn: "Id");
        }
    }
}
