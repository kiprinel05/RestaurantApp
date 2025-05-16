using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class AddTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PrepTime",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Italian style pizzas", "Pizza" },
                    { 2, "Fresh homemade pasta", "Pasta" },
                    { 3, "Sweet treats", "Desserts" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "IsAvailable", "Name", "PortionQuantity", "PrepTime", "Price", "TotalQuantity" },
                values: new object[,]
                {
                    { 1, 1, "Classic pizza with tomato sauce, mozzarella, and basil", true, "Margherita Pizza", 0, 20, 45.00m, 0 },
                    { 2, 2, "Spaghetti with eggs, pecorino cheese, guanciale, and black pepper", true, "Carbonara", 0, 15, 35.00m, 0 },
                    { 3, 3, "Classic Italian dessert with coffee-soaked ladyfingers and mascarpone cream", true, "Tiramisu", 0, 10, 25.00m, 0 },
                    { 4, 1, "Pizza with four different types of cheese", true, "Quattro Formaggi", 0, 20, 50.00m, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PrepTime",
                table: "Products");
        }
    }
}
