using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuzKit3D.Modules.Cart.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cart");

            migrationBuilder.CreateTable(
                name: "carts",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cart_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    total_item = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_carts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "in_stock_inventory_replicas",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    in_stock_product_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_quantity = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_in_stock_inventory_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "in_stock_price_replicas",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    effective_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    effective_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_in_stock_price_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "in_stock_product_price_detail_replicas",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    in_stock_price_id = table.Column<Guid>(type: "uuid", nullable: false),
                    in_stock_product_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_in_stock_product_price_detail_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "in_stock_product_replicas",
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
                    table.PrimaryKey("pk_in_stock_product_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "in_stock_product_variant_replicas",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    in_stock_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sku = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    color = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    assembled_length_mm = table.Column<int>(type: "integer", nullable: false),
                    assembled_width_mm = table.Column<int>(type: "integer", nullable: false),
                    assembled_height_mm = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_in_stock_product_variant_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "partner_product_replicas",
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
                    table.PrimaryKey("pk_partner_product_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_replicas",
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
                    table.PrimaryKey("pk_user_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cart_items",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cart_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    unit_price_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    in_stock_product_price_detail_id = table.Column<Guid>(type: "uuid", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cart_items", x => x.id);
                    table.ForeignKey(
                        name: "FK__cart__cart_item",
                        column: x => x.cart_id,
                        principalSchema: "cart",
                        principalTable: "carts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "CUK___cart___cart_id__item_id",
                schema: "cart",
                table: "cart_items",
                columns: new[] { "cart_id", "item_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CUK___cart___user_id__cart_type",
                schema: "cart",
                table: "carts",
                columns: new[] { "user_id", "cart_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_instock_inventory_replica_instock_product_variant_id",
                schema: "cart",
                table: "in_stock_inventory_replicas",
                column: "in_stock_product_variant_id");

            migrationBuilder.CreateIndex(
                name: "CUK___instock_product_price_detail_replica___instock_price_id__instock_product_variant_id",
                schema: "cart",
                table: "in_stock_product_price_detail_replicas",
                columns: new[] { "in_stock_price_id", "in_stock_product_variant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__instock_product_replica__slug",
                schema: "cart",
                table: "in_stock_product_replicas",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__instock_product_variant_replica__sku",
                schema: "cart",
                table: "in_stock_product_variant_replicas",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CUK___partner_product_replica___partner_id__partner_product_sku",
                schema: "cart",
                table: "partner_product_replicas",
                columns: new[] { "partner_id", "partner_product_sku" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__partner_product_replica__slug",
                schema: "cart",
                table: "partner_product_replicas",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__user_replica__email",
                schema: "cart",
                table: "user_replicas",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__user_replica__phone_number",
                schema: "cart",
                table: "user_replicas",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart_items",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "in_stock_inventory_replicas",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "in_stock_price_replicas",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "in_stock_product_price_detail_replicas",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "in_stock_product_replicas",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "in_stock_product_variant_replicas",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "partner_product_replicas",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "user_replicas",
                schema: "cart");

            migrationBuilder.DropTable(
                name: "carts",
                schema: "cart");
        }
    }
}
