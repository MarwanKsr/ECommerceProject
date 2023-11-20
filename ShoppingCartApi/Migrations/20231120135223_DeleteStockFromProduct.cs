using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCardApi.Migrations
{
    /// <inheritdoc />
    public partial class DeleteStockFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_CartHeaders_CartHeaderId",
                table: "CartDetails");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CartHeaderId",
                table: "CartDetails",
                newName: "CardHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_CartDetails_CartHeaderId",
                table: "CartDetails",
                newName: "IX_CartDetails_CardHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_CartHeaders_CardHeaderId",
                table: "CartDetails",
                column: "CardHeaderId",
                principalTable: "CartHeaders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_CartHeaders_CardHeaderId",
                table: "CartDetails");

            migrationBuilder.RenameColumn(
                name: "CardHeaderId",
                table: "CartDetails",
                newName: "CartHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_CartDetails_CardHeaderId",
                table: "CartDetails",
                newName: "IX_CartDetails_CartHeaderId");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_CartHeaders_CartHeaderId",
                table: "CartDetails",
                column: "CartHeaderId",
                principalTable: "CartHeaders",
                principalColumn: "Id");
        }
    }
}
