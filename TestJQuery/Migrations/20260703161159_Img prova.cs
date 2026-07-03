using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestJQuery.Migrations
{
    /// <inheritdoc />
    public partial class Imgprova : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Image",
                value: "TestJQuery/wwwroot/src/Margherita.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Image",
                value: "~/src/Margherita.jpg");
        }
    }
}
