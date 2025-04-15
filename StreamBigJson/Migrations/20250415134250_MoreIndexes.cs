using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamBigJson.Migrations
{
    /// <inheritdoc />
    public partial class MoreIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PackageDependencies_PackageVersionUuid",
                table: "PackageDependencies",
                column: "PackageVersionUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PackageCategories_CategoryLabel",
                table: "PackageCategories",
                column: "CategoryLabel");

            migrationBuilder.CreateIndex(
                name: "IX_PackageCategories_PackageUuid",
                table: "PackageCategories",
                column: "PackageUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PackageDependencies_PackageVersionUuid",
                table: "PackageDependencies");

            migrationBuilder.DropIndex(
                name: "IX_PackageCategories_CategoryLabel",
                table: "PackageCategories");

            migrationBuilder.DropIndex(
                name: "IX_PackageCategories_PackageUuid",
                table: "PackageCategories");
        }
    }
}
