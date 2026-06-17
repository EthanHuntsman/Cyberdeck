using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cyberdeck.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDecks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeckCards_DeckId",
                table: "DeckCards");

            migrationBuilder.CreateIndex(
                name: "IX_DeckCards_DeckId_CardId",
                table: "DeckCards",
                columns: new[] { "DeckId", "CardId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeckCards_DeckId_CardId",
                table: "DeckCards");

            migrationBuilder.CreateIndex(
                name: "IX_DeckCards_DeckId",
                table: "DeckCards",
                column: "DeckId");
        }
    }
}
