using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCardApi.Migrations
{
    /// <inheritdoc />
    public partial class AddProductIdToProductEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Products");
        }
    }
}
