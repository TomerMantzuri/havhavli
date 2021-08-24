using Microsoft.EntityFrameworkCore.Migrations;

namespace havhavli.Migrations
{
    public partial class FixCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CartItem_ProductID",
                table: "CartItem");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ProductID",
                table: "CartItem",
                column: "ProductID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CartItem_ProductID",
                table: "CartItem");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ProductID",
                table: "CartItem",
                column: "ProductID");
        }
    }
}
