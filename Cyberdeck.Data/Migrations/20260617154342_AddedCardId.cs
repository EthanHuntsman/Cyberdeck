using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cyberdeck.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCardId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardId",
                table: "Cards",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Cards");
        }
    }
}
