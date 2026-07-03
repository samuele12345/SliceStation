using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestJQuery.Migrations
{
    /// <inheritdoc />
    public partial class urlimgmodificato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Image",
                value: "/src/Margherita.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Image",
                value: "/src/Margherita.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Image",
                value: "/src/Margherita.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 4,
                column: "Image",
                value: "/src/Margherita.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 5,
                column: "Image",
                value: "/src/Margherita.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Image",
                value: "TestJQuery\\wwwroot\\src\\Margherita.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Image",
                value: "TestJQuery\\wwwroot\\src\\Margherita.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Image",
                value: "TestJQuery\\wwwroot\\src\\Margherita.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 4,
                column: "Image",
                value: "TestJQuery\\wwwroot\\src\\Margherita.jpg");

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 5,
                column: "Image",
                value: "TestJQuery\\wwwroot\\src\\Margherita.jpg");
        }
    }
}
