using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PuzKit3D.Modules.Cart.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cart");

            migrationBuilder.CreateTable(
                name: "cart_type",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cart_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "instock_product_price_detail_replica",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    instock_price_id = table.Column<Guid>(type: "uuid", nullable: false),
                    instock_product_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instock_product_price_detail_replica", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "instock_product_replica",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    brief_description = table.Column<string>(type: "text", nullable: true),
                    detail_description = table.Column<string>(type: "text", nullable: true),
                    difficult_level = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    estimated_build_time = table.Column<int>(type: "integer", nullable: false),
                    thumbnail_url = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    specification = table.Column<string>(type: "jsonb", nullable: true),
                    preview_asset = table.Column<string>(type: "jsonb", nullable: false),
                    topic_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assembly_method = table.Column<Guid>(type: "uuid", nullable: false),
                    capability = table.Column<Guid>(type: "uuid", nullable: false),
                    material = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instock_product_replica", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "instock_product_variant_replica",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    instock_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sku = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    color = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    size = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instock_product_variant_replica", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "partner_product_replica",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    partner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    partner_product_sku = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    brief_description = table.Column<string>(type: "text", nullable: true),
                    detail_description = table.Column<string>(type: "text", nullable: true),
                    product_catalog = table.Column<string>(type: "jsonb", nullable: true),
                    thumbnail_url = table.Column<string>(type: "text", nullable: false),
                    preview_asset = table.Column<string>(type: "jsonb", nullable: false),
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_product_replica", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_replica",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    address = table.Column<string>(type: "jsonb", nullable: false),
                    phone_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_replica", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cart",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cart_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    totalItem = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cart", x => x.id);
                    table.ForeignKey(
                        name: "FK__cart__cart_type",
                        column: x => x.cart_type_id,
                        principalSchema: "cart",
                        principalTable: "cart_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cart_item",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cart_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    instock_product_price_detail_id = table.Column<Guid>(type: "uuid", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cart_item", x => x.id);
                    table.ForeignKey(
                        name: "FK__cart__cart_item",
                        column: x => x.cart_id,
                        principalSchema: "cart",
                        principalTable: "cart",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "cart",
                table: "cart_type",
                columns: new[] { "id", "created_at", "is_active", "name", "updated_at" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "INSTOCK", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "PARTNER", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "CUK___cart___user_id__cart_type_id",
                schema: "cart",
                table: "cart",
                columns: new[] { "user_id", "cart_type_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_cart_cart_type_id",
                schema: "cart",
                table: "cart",
                column: "cart_type_id");

            migrationBuilder.CreateIndex(
                name: "CUK___cart___cart_id__item_id",
                schema: "cart",
                table: "cart_item",
                columns: new[] { "cart_id", "item_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK___cart_type_name",
                schema: "cart",
                table: "cart_type",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CUK___instock_product_price_detail_replica___instock_price_id__instock_product_variant_id",
                schema: "cart",
                table: "instock_product_price_detail_replica",
                columns: new[] { "instock_price_id", "instock_product_variant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__instock_product_replica__slug",
                schema: "cart",
                table: "instock_product_replica",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CUK___instock_product_variant_replica___instock_product_id__color__size",
                schema: "cart",
                table: "instock_product_variant_replica",
                columns: new[] { "instock_product_id", "color", "size" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__instock_product_variant_replica__sku",
                schema: "cart",
                table: "instock_product_variant_replica",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CUK___partner_product_replica___partner_id__partner_product_sku",
                schema: "cart",
                table: "partner_product_replica",
                columns: new[] { "partner_id", "partner_product_sku" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__partner_product_replica__slug",
                schema: "cart",
                table: "partner_product_replica",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__user_replica__email",
                schema: "cart",
                table: "user_replica",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__user_replica__phone_number",
                schema: "cart",
                table: "user_replica",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart_item",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "instock_product_price_detail_replica",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "instock_product_replica",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "instock_product_variant_replica",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "partner_product_replica",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "user_replica",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "cart",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "cart_type",
                schema: "cart");
        }
    }
}
