using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Migrations
{
    /// <inheritdoc />
    public partial class RecreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "LastName" },
                values: new object[] { "customer@test.com", "Customer" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DeliveryAddress", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Role" },
                values: new object[] { 2, "Restaurant Address", "employee@test.com", "Test", "Employee", "7NcYcNGWMxapfjrDQIyYNa2M8PPBvHA1J8MCZVNPda4=", "0733333333", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "LastName" },
                values: new object[] { "test@test.com", "User" });
        }
    }
}
