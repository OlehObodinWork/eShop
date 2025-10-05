using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pgvector;

#nullable disable

namespace eShop.Catalog.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PrimaryModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrimaryCatalogFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryCatalogFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryCatalogItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OriginName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    ProductWeight = table.Column<string>(type: "text", nullable: true),
                    ProducctType = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<string>(type: "text", nullable: true),
                    CategoryName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ProductSKU = table.Column<string>(type: "text", nullable: true),
                    ProductKey = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    OriginPrice = table.Column<string>(type: "text", nullable: true),
                    SuggestSellPrice = table.Column<string>(type: "text", nullable: true),
                    ListedNum = table.Column<int>(type: "integer", nullable: false),
                    PictureFileName = table.Column<string>(type: "text", nullable: true),
                    PackingWeight = table.Column<string>(type: "text", nullable: true),
                    PackingName = table.Column<string>(type: "text", nullable: true),
                    PackingNameSet = table.Column<string>(type: "text", nullable: true),
                    CatalogTypeId = table.Column<int>(type: "integer", nullable: false),
                    CatalogBrandId = table.Column<int>(type: "integer", nullable: false),
                    AvailableStock = table.Column<int>(type: "integer", nullable: false),
                    RestockThreshold = table.Column<int>(type: "integer", nullable: false),
                    MaxStockThreshold = table.Column<int>(type: "integer", nullable: false),
                    Embedding = table.Column<Vector>(type: "vector", nullable: true),
                    OnReorder = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryCatalogItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryCatalogKits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryCatalogKits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryCatalogEnhancedImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Src = table.Column<string>(type: "text", nullable: true),
                    PrimaryCatalogItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryCatalogEnhancedImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimaryCatalogEnhancedImages_PrimaryCatalogItems_PrimaryCat~",
                        column: x => x.PrimaryCatalogItemId,
                        principalTable: "PrimaryCatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryCatalogFeatureValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PrimaryCatalogItemId = table.Column<int>(type: "integer", nullable: false),
                    PrimaryCatalogFeatureId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryCatalogFeatureValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimaryCatalogFeatureValues_PrimaryCatalogFeatures_PrimaryC~",
                        column: x => x.PrimaryCatalogFeatureId,
                        principalTable: "PrimaryCatalogFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimaryCatalogFeatureValues_PrimaryCatalogItems_PrimaryCata~",
                        column: x => x.PrimaryCatalogItemId,
                        principalTable: "PrimaryCatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryCatalogItemVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    ProudctIdString = table.Column<string>(type: "text", nullable: true),
                    VariantId = table.Column<string>(type: "text", nullable: true),
                    VariantImageOrigin = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    VarianImageEnhanced = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    VariantSKU = table.Column<string>(type: "text", nullable: true),
                    VariantKey = table.Column<string>(type: "text", nullable: true),
                    VarianKeyAdjust = table.Column<string>(type: "text", nullable: true),
                    VariantLength = table.Column<decimal>(type: "numeric", nullable: false),
                    VariantHeigt = table.Column<decimal>(type: "numeric", nullable: false),
                    VariantWith = table.Column<decimal>(type: "numeric", nullable: false),
                    VariatVolume = table.Column<decimal>(type: "numeric", nullable: false),
                    VariantPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    VariantSellPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    VariantStandart = table.Column<string>(type: "text", nullable: true),
                    VariantFinallPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    PrimaryCatalogItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryCatalogItemVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimaryCatalogItemVariants_PrimaryCatalogItems_PrimaryCatal~",
                        column: x => x.PrimaryCatalogItemId,
                        principalTable: "PrimaryCatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryCatalogOriginalImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Src = table.Column<string>(type: "text", nullable: true),
                    PrimaryCatalogItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryCatalogOriginalImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimaryCatalogOriginalImages_PrimaryCatalogItems_PrimaryCat~",
                        column: x => x.PrimaryCatalogItemId,
                        principalTable: "PrimaryCatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryCatalogItemPrimaryCatalogKit",
                columns: table => new
                {
                    PrimaryCatalogItemsId = table.Column<int>(type: "integer", nullable: false),
                    PrimaryCatalogKitsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryCatalogItemPrimaryCatalogKit", x => new { x.PrimaryCatalogItemsId, x.PrimaryCatalogKitsId });
                    table.ForeignKey(
                        name: "FK_PrimaryCatalogItemPrimaryCatalogKit_PrimaryCatalogItems_Pri~",
                        column: x => x.PrimaryCatalogItemsId,
                        principalTable: "PrimaryCatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimaryCatalogItemPrimaryCatalogKit_PrimaryCatalogKits_Prim~",
                        column: x => x.PrimaryCatalogKitsId,
                        principalTable: "PrimaryCatalogKits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryCatalogEnhancedImages_PrimaryCatalogItemId",
                table: "PrimaryCatalogEnhancedImages",
                column: "PrimaryCatalogItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryCatalogFeatureValues_PrimaryCatalogFeatureId",
                table: "PrimaryCatalogFeatureValues",
                column: "PrimaryCatalogFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryCatalogFeatureValues_PrimaryCatalogItemId",
                table: "PrimaryCatalogFeatureValues",
                column: "PrimaryCatalogItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryCatalogItemPrimaryCatalogKit_PrimaryCatalogKitsId",
                table: "PrimaryCatalogItemPrimaryCatalogKit",
                column: "PrimaryCatalogKitsId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryCatalogItemVariants_PrimaryCatalogItemId",
                table: "PrimaryCatalogItemVariants",
                column: "PrimaryCatalogItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryCatalogOriginalImages_PrimaryCatalogItemId",
                table: "PrimaryCatalogOriginalImages",
                column: "PrimaryCatalogItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrimaryCatalogEnhancedImages");

            migrationBuilder.DropTable(
                name: "PrimaryCatalogFeatureValues");

            migrationBuilder.DropTable(
                name: "PrimaryCatalogItemPrimaryCatalogKit");

            migrationBuilder.DropTable(
                name: "PrimaryCatalogItemVariants");

            migrationBuilder.DropTable(
                name: "PrimaryCatalogOriginalImages");

            migrationBuilder.DropTable(
                name: "PrimaryCatalogFeatures");

            migrationBuilder.DropTable(
                name: "PrimaryCatalogKits");

            migrationBuilder.DropTable(
                name: "PrimaryCatalogItems");
        }
    }
}
