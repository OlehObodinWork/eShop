using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDEFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameDE",
                table: "CatalogKits");

            migrationBuilder.DropColumn(
                name: "VarianKeyEnAdjusted",
                table: "CatalogItemVariants");

            migrationBuilder.DropColumn(
                name: "CategoryNameDE",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "CategoryNameEN",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "DescriptionDE",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "DescriptionEN",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "NameDE",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "PackingNameDE",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "PackingNameEN",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "PackingNameSetDE",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "ValueDE",
                table: "CatalogFeatureValues");

            migrationBuilder.DropColumn(
                name: "TitleDE",
                table: "CatalogFeatures");

            migrationBuilder.RenameColumn(
                name: "VariantKeyEN",
                table: "CatalogItemVariants",
                newName: "VariantKey");

            migrationBuilder.RenameColumn(
                name: "VariantKeyDE",
                table: "CatalogItemVariants",
                newName: "VarianKeyAdjust");

            migrationBuilder.RenameColumn(
                name: "ProductKeyEN",
                table: "CatalogItems",
                newName: "ProductKey");

            migrationBuilder.RenameColumn(
                name: "ProductKenDE",
                table: "CatalogItems",
                newName: "PackingNameSet");

            migrationBuilder.RenameColumn(
                name: "PackingNameSetEN",
                table: "CatalogItems",
                newName: "PackingName");

            migrationBuilder.RenameColumn(
                name: "NameEN",
                table: "CatalogItems",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "ValueEN",
                table: "CatalogFeatureValues",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "TitleEN",
                table: "CatalogFeatures",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VariantKey",
                table: "CatalogItemVariants",
                newName: "VariantKeyEN");

            migrationBuilder.RenameColumn(
                name: "VarianKeyAdjust",
                table: "CatalogItemVariants",
                newName: "VariantKeyDE");

            migrationBuilder.RenameColumn(
                name: "ProductKey",
                table: "CatalogItems",
                newName: "ProductKeyEN");

            migrationBuilder.RenameColumn(
                name: "PackingNameSet",
                table: "CatalogItems",
                newName: "ProductKenDE");

            migrationBuilder.RenameColumn(
                name: "PackingName",
                table: "CatalogItems",
                newName: "PackingNameSetEN");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "CatalogItems",
                newName: "NameEN");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "CatalogFeatureValues",
                newName: "ValueEN");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "CatalogFeatures",
                newName: "TitleEN");

            migrationBuilder.AddColumn<string>(
                name: "NameDE",
                table: "CatalogKits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VarianKeyEnAdjusted",
                table: "CatalogItemVariants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryNameDE",
                table: "CatalogItems",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryNameEN",
                table: "CatalogItems",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDE",
                table: "CatalogItems",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEN",
                table: "CatalogItems",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDE",
                table: "CatalogItems",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackingNameDE",
                table: "CatalogItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackingNameEN",
                table: "CatalogItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackingNameSetDE",
                table: "CatalogItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValueDE",
                table: "CatalogFeatureValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleDE",
                table: "CatalogFeatures",
                type: "text",
                nullable: true);
        }
    }
}
