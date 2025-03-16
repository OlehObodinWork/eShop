using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCatalogFeatureRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogFeatureCatalogItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatalogFeatureCatalogItem",
                columns: table => new
                {
                    CatalogFeaturesId = table.Column<int>(type: "integer", nullable: false),
                    CatalogItemsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogFeatureCatalogItem", x => new { x.CatalogFeaturesId, x.CatalogItemsId });
                    table.ForeignKey(
                        name: "FK_CatalogFeatureCatalogItem_CatalogFeatures_CatalogFeaturesId",
                        column: x => x.CatalogFeaturesId,
                        principalTable: "CatalogFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogFeatureCatalogItem_CatalogItems_CatalogItemsId",
                        column: x => x.CatalogItemsId,
                        principalTable: "CatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogFeatureCatalogItem_CatalogItemsId",
                table: "CatalogFeatureCatalogItem",
                column: "CatalogItemsId");
        }
    }
}
