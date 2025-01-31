using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameCatalogFeatureValuesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "CatalogFeatureValues",
                newName: "CatalogFeatureValue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "CatalogFeatureValue",
                newName: "CatalogFeatureValues");
        }
    }
}
