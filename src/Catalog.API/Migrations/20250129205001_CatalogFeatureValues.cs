using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Catalog.API.Migrations
{
    /// <inheritdoc />
    public partial class CatalogFeatureValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueDE",
                table: "CatalogFeatures");

            migrationBuilder.DropColumn(
                name: "ValueEN",
                table: "CatalogFeatures");

            migrationBuilder.CreateTable(
                name: "CatalogFeatureValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CatalogItemId = table.Column<int>(type: "integer", nullable: false),
                    CatalogFeatureId = table.Column<int>(type: "integer", nullable: false),
                    ValueEN = table.Column<string>(type: "text", nullable: true),
                    ValueDE = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogFeatureValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogFeatureValues_CatalogFeatures_CatalogFeatureId",
                        column: x => x.CatalogFeatureId,
                        principalTable: "CatalogFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogFeatureValues_CatalogItems_CatalogItemId",
                        column: x => x.CatalogItemId,
                        principalTable: "CatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogFeatureValues_CatalogFeatureId",
                table: "CatalogFeatureValues",
                column: "CatalogFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogFeatureValues_CatalogItemId",
                table: "CatalogFeatureValues",
                column: "CatalogItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogFeatureValues");

            migrationBuilder.AddColumn<string>(
                name: "ValueDE",
                table: "CatalogFeatures",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValueEN",
                table: "CatalogFeatures",
                type: "text",
                nullable: true);
        }
    }
}
