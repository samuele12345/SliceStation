using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestJQuery.Migrations
{
    /// <inheritdoc />
    public partial class aggiuntoOrderedPizzaalcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedPizza_Orders_OrderId",
                table: "OrderedPizza");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedPizza_Pizzas_OrderId",
                table: "OrderedPizza");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedPizza",
                table: "OrderedPizza");

            migrationBuilder.RenameTable(
                name: "OrderedPizza",
                newName: "OrderedPizzas");

            migrationBuilder.RenameIndex(
                name: "IX_OrderedPizza_OrderId",
                table: "OrderedPizzas",
                newName: "IX_OrderedPizzas_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedPizzas",
                table: "OrderedPizzas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedPizzas_Orders_OrderId",
                table: "OrderedPizzas",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedPizzas_Pizzas_OrderId",
                table: "OrderedPizzas",
                column: "OrderId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedPizzas_Orders_OrderId",
                table: "OrderedPizzas");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedPizzas_Pizzas_OrderId",
                table: "OrderedPizzas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedPizzas",
                table: "OrderedPizzas");

            migrationBuilder.RenameTable(
                name: "OrderedPizzas",
                newName: "OrderedPizza");

            migrationBuilder.RenameIndex(
                name: "IX_OrderedPizzas_OrderId",
                table: "OrderedPizza",
                newName: "IX_OrderedPizza_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedPizza",
                table: "OrderedPizza",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedPizza_Orders_OrderId",
                table: "OrderedPizza",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedPizza_Pizzas_OrderId",
                table: "OrderedPizza",
                column: "OrderId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
