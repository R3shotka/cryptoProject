using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityToUserAssetBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d39921b7-994e-468b-a2ef-075757acbd4a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec56da53-d23a-44e3-86bf-ad242f65d982");

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "UserAssetBalances",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b25c651-061b-4745-8504-9027cef98de6", null, "User", "USER" },
                    { "da276984-0025-4740-aa73-d6b30a9fcca5", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b25c651-061b-4745-8504-9027cef98de6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da276984-0025-4740-aa73-d6b30a9fcca5");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "UserAssetBalances");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d39921b7-994e-468b-a2ef-075757acbd4a", null, "Admin", "ADMIN" },
                    { "ec56da53-d23a-44e3-86bf-ad242f65d982", null, "User", "USER" }
                });
        }
    }
}
