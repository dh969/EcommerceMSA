using Microsoft.EntityFrameworkCore.Migrations;

namespace cartandorderService.Data.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Cart");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Cart",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
