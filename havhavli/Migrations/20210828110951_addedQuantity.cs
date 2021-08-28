using Microsoft.EntityFrameworkCore.Migrations;

namespace havhavli.Migrations
{
    public partial class addedQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopQuantity",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopQuantity",
                table: "Product");
        }
    }
}
