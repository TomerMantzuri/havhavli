using Microsoft.EntityFrameworkCore.Migrations;

namespace havhavli.Migrations
{
    public partial class fixedsProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "SupplierProducts");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "SupplierProducts",
                newName: "InStock");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InStock",
                table: "SupplierProducts",
                newName: "Quantity");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "SupplierProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
