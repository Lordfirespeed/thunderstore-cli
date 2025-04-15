using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamBigJson.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PackageVersions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PackageVersions",
                type: "TEXT",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "DownloadCount",
                table: "PackageVersions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DownloadUrl",
                table: "PackageVersions",
                type: "TEXT",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "PackageVersions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "PackageVersions",
                type: "TEXT",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PackageVersions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "PackageVersions",
                type: "TEXT",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Packages",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DonationUrl",
                table: "Packages",
                type: "TEXT",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasNsfwContent",
                table: "Packages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeprecated",
                table: "Packages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPinned",
                table: "Packages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PackageUrl",
                table: "Packages",
                type: "TEXT",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "RatingScore",
                table: "Packages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Packages",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "PackageCategories",
                columns: table => new
                {
                    PackageUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryLabel = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageCategories", x => new { x.PackageUuid, x.CategoryLabel });
                    table.ForeignKey(
                        name: "FK_PackageCategories_Packages_PackageUuid",
                        column: x => x.PackageUuid,
                        principalTable: "Packages",
                        principalColumn: "PackageUuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageDependencies",
                columns: table => new
                {
                    PackageVersionUuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    DependencyNamespace = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    DependencyName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    DependencyVersionString = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageDependencies", x => new { x.PackageVersionUuid, x.DependencyNamespace, x.DependencyName });
                    table.ForeignKey(
                        name: "FK_PackageDependencies_PackageVersions_PackageVersionUuid",
                        column: x => x.PackageVersionUuid,
                        principalTable: "PackageVersions",
                        principalColumn: "PackageVersionUuid",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageCategories");

            migrationBuilder.DropTable(
                name: "PackageDependencies");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PackageVersions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PackageVersions");

            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "PackageVersions");

            migrationBuilder.DropColumn(
                name: "DownloadUrl",
                table: "PackageVersions");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "PackageVersions");

            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "PackageVersions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PackageVersions");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "PackageVersions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "DonationUrl",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "HasNsfwContent",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "IsDeprecated",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "IsPinned",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "PackageUrl",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "RatingScore",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Packages");
        }
    }
}
