using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cyberdeck.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCardImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Cards",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Cards");
        }
    }
}
