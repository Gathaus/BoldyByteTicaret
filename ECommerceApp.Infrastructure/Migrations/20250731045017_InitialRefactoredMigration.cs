using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialRefactoredMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Slug",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "TextColor",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "ProductViews");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "AttributeValues");

            migrationBuilder.RenameColumn(
                name: "AddedAt",
                table: "WishlistItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "IsVerifiedPurchase",
                table: "ProductReviews",
                newName: "IsVerified");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Tags",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Tags",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Tags",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Referrer",
                table: "ProductViews",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ProductTags",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductTags",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsHelpful",
                table: "ProductReviews",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ProductImages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductImages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductAttributes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ProductAttributes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductAttributes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CategoryAttributes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CategoryAttributes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CategoryAttributes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AttributeValues",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "AttributeValues",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AttributeValues",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Attributes",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Referrer",
                table: "ProductViews");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ProductTags");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductTags");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsHelpful",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductAttributes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ProductAttributes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductAttributes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CategoryAttributes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CategoryAttributes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CategoryAttributes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Attributes");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "WishlistItems",
                newName: "AddedAt");

            migrationBuilder.RenameColumn(
                name: "IsVerified",
                table: "ProductReviews",
                newName: "IsVerifiedPurchase");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Wishlists",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "WishlistItems",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Tags",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Tags",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Tags",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextColor",
                table: "Tags",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "ProductViews",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "ProductVariants",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "ProductVariants",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Products",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "Products",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "AttributeValues",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Slug",
                table: "Tags",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");
        }
    }
}
