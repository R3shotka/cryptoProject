using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class UserbalancesManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eecf417e-73f3-4c44-92ad-cf32fd016708");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f24514ed-bdc3-44b6-8596-3caf60c7769a");

            migrationBuilder.CreateTable(
                name: "UserAssetBalances",
                columns: table => new
                {
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CryptoAssetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssetBalances", x => new { x.AppUserId, x.CryptoAssetId });
                    table.ForeignKey(
                        name: "FK_UserAssetBalances_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAssetBalances_CryptoAssets_CryptoAssetId",
                        column: x => x.CryptoAssetId,
                        principalTable: "CryptoAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d39921b7-994e-468b-a2ef-075757acbd4a", null, "Admin", "ADMIN" },
                    { "ec56da53-d23a-44e3-86bf-ad242f65d982", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAssetBalances_CryptoAssetId",
                table: "UserAssetBalances",
                column: "CryptoAssetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAssetBalances");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d39921b7-994e-468b-a2ef-075757acbd4a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec56da53-d23a-44e3-86bf-ad242f65d982");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "eecf417e-73f3-4c44-92ad-cf32fd016708", null, "Admin", "ADMIN" },
                    { "f24514ed-bdc3-44b6-8596-3caf60c7769a", null, "User", "USER" }
                });
        }
    }
}
