using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderApi.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderHeaderOrderDetailsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId1",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderHeaderId1",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "OrderHeaderId1",
                table: "OrderDetails");

            migrationBuilder.CreateTable(
                name: "OrderHeaderOrderDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    OrderDetailsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHeaderOrderDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderHeaderOrderDetails");

            migrationBuilder.AddColumn<long>(
                name: "OrderHeaderId1",
                table: "OrderDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderHeaderId1",
                table: "OrderDetails",
                column: "OrderHeaderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId1",
                table: "OrderDetails",
                column: "OrderHeaderId1",
                principalTable: "OrderHeaders",
                principalColumn: "Id");
        }
    }
}
