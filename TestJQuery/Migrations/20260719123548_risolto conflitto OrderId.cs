using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestJQuery.Migrations
{
    /// <inheritdoc />
    public partial class risoltoconflittoOrderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedPizzas_Pizzas_OrderId",
                table: "OrderedPizzas");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedPizzas_PizzaId",
                table: "OrderedPizzas",
                column: "PizzaId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedPizzas_Pizzas_PizzaId",
                table: "OrderedPizzas",
                column: "PizzaId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedPizzas_Pizzas_PizzaId",
                table: "OrderedPizzas");

            migrationBuilder.DropIndex(
                name: "IX_OrderedPizzas_PizzaId",
                table: "OrderedPizzas");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedPizzas_Pizzas_OrderId",
                table: "OrderedPizzas",
                column: "OrderId",
                principalTable: "Pizzas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
