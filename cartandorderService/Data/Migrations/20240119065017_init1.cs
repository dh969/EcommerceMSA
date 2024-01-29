using Microsoft.EntityFrameworkCore.Migrations;

namespace cartandorderService.Data.Migrations
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ProductBrand",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "AvailableQuantity",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "ProductBrand",
                table: "Cart");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductBrand",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvailableQuantity",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cart",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Cart",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Cart",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductBrand",
                table: "Cart",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
