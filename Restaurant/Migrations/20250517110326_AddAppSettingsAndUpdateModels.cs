using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class AddAppSettingsAndUpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Menus",
                newName: "BasePrice");

            migrationBuilder.AddColumn<int>(
                name: "MenuSpecificPortionQuantity",
                table: "MenuProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuDiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OrderDiscountThreshold = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderCountForDiscount = table.Column<int>(type: "int", nullable: false),
                    OrderTimeWindowHours = table.Column<int>(type: "int", nullable: false),
                    FreeDeliveryThreshold = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LowStockThreshold = table.Column<int>(type: "int", nullable: false),
                    OrderDiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    EstimatedDeliveryTimeMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppSettings",
                columns: new[] { "Id", "DeliveryCost", "EstimatedDeliveryTimeMinutes", "FreeDeliveryThreshold", "LowStockThreshold", "MenuDiscountPercentage", "OrderCountForDiscount", "OrderDiscountPercentage", "OrderDiscountThreshold", "OrderTimeWindowHours" },
                values: new object[] { 1, 15.00m, 45, 150.00m, 10, 10.00m, 3, 15.00m, 200.00m, 24 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PortionQuantity", "TotalQuantity" },
                values: new object[] { 450, 4500 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PortionQuantity", "TotalQuantity" },
                values: new object[] { 350, 3500 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PortionQuantity", "TotalQuantity" },
                values: new object[] { 200, 2000 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PortionQuantity", "TotalQuantity" },
                values: new object[] { 450, 4500 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MenuSpecificPortionQuantity",
                table: "MenuProducts");

            migrationBuilder.RenameColumn(
                name: "BasePrice",
                table: "Menus",
                newName: "Price");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PortionQuantity", "TotalQuantity" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PortionQuantity", "TotalQuantity" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PortionQuantity", "TotalQuantity" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PortionQuantity", "TotalQuantity" },
                values: new object[] { 0, 0 });
        }
    }
}
