using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookDemo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sold",
                table: "Carts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Carts");
        }
    }
}
