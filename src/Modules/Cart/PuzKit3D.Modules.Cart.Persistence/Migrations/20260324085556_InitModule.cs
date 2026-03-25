using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    total_piece_count = table.Column<int>(type: "integer", nullable: false),
                    difficult_level = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    estimated_build_time = table.Column<int>(type: "integer", nullable: false),
                    thumbnail_url = table.Column<string>(type: "text", nullable: false),
                    preview_asset = table.Column<string>(type: "jsonb", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    topic_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assembly_method_id = table.Column<Guid>(type: "uuid", nullable: false),
                    material_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    reference_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
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

            migrationBuilder.InsertData(
                schema: "cart",
                table: "in_stock_inventory_replicas",
                columns: new[] { "id", "created_at", "in_stock_product_variant_id", "total_quantity", "updated_at" },
                values: new object[,]
                {
                    { new Guid("40000000-0001-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0001-0000-0000-000000000001"), 75, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0002-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0001-0000-0000-000000000002"), 83, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0003-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0002-0000-0000-000000000001"), 162, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0004-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0003-0000-0000-000000000001"), 152, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0005-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0003-0000-0000-000000000002"), 123, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0006-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0004-0000-0000-000000000001"), 167, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0007-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0004-0000-0000-000000000002"), 52, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0008-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0005-0000-0000-000000000001"), 99, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0009-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0006-0000-0000-000000000001"), 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0010-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0006-0000-0000-000000000002"), 186, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0011-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0007-0000-0000-000000000001"), 78, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0012-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0008-0000-0000-000000000001"), 143, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0013-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0008-0000-0000-000000000002"), 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0014-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0009-0000-0000-000000000001"), 117, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0015-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0010-0000-0000-000000000001"), 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0016-0000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0010-0000-0000-000000000002"), 185, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "cart",
                table: "in_stock_price_replicas",
                columns: new[] { "id", "created_at", "effective_from", "effective_to", "is_active", "name", "priority", "updated_at" },
                values: new object[,]
                {
                    { new Guid("99999999-9999-9999-9999-999999999991"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2099, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), true, "Standard", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("99999999-9999-9999-9999-999999999992"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 23, 59, 59, 0, DateTimeKind.Utc), true, "Sale Summer Vacation", 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "cart",
                table: "in_stock_product_price_detail_replicas",
                columns: new[] { "id", "created_at", "in_stock_price_id", "in_stock_product_variant_id", "is_active", "unit_price", "updated_at" },
                values: new object[,]
                {
                    { new Guid("30000000-0001-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0001-0000-0000-000000000001"), true, 745000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0001-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0001-0000-0000-000000000001"), true, 596000.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0002-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0001-0000-0000-000000000002"), true, 419000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0002-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0001-0000-0000-000000000002"), true, 335200.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0003-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0002-0000-0000-000000000001"), true, 268000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0003-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0002-0000-0000-000000000001"), true, 214400.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0004-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0003-0000-0000-000000000001"), true, 869000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0004-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0003-0000-0000-000000000001"), true, 695200.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0005-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0003-0000-0000-000000000002"), true, 648000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0005-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0003-0000-0000-000000000002"), true, 518400.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0006-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0004-0000-0000-000000000001"), true, 356000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0006-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0004-0000-0000-000000000001"), true, 284800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0007-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0004-0000-0000-000000000002"), true, 909000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0007-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0004-0000-0000-000000000002"), true, 727200.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0008-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0005-0000-0000-000000000001"), true, 631000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0008-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0005-0000-0000-000000000001"), true, 504800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0009-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0006-0000-0000-000000000001"), true, 531000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0009-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0006-0000-0000-000000000001"), true, 424800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0010-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0006-0000-0000-000000000002"), true, 615000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0010-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0006-0000-0000-000000000002"), true, 492000.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0011-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0007-0000-0000-000000000001"), true, 704000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0011-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0007-0000-0000-000000000001"), true, 563200.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0012-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0008-0000-0000-000000000001"), true, 540000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0012-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0008-0000-0000-000000000001"), true, 432000.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0013-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0008-0000-0000-000000000002"), true, 330000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0013-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0008-0000-0000-000000000002"), true, 264000.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0014-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0009-0000-0000-000000000001"), true, 42000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0014-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0009-0000-0000-000000000001"), true, 33600.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0015-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0010-0000-0000-000000000001"), true, 867000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0015-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0010-0000-0000-000000000001"), true, 693600.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0016-1000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("20000000-0010-0000-0000-000000000002"), true, 509000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0016-2000-0000-000000000000"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("20000000-0010-0000-0000-000000000002"), true, 407200.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "cart",
                table: "in_stock_product_replicas",
                columns: new[] { "id", "assembly_method_id", "code", "created_at", "description", "difficult_level", "estimated_build_time", "is_active", "material_id", "name", "preview_asset", "slug", "thumbnail_url", "topic_id", "total_piece_count", "updated_at" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), "INP001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!", "Basic", 120, true, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), "UGT-24 Endurance Racer", "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}", "ugt-24-endurance-racer", "instock-products/ugt-24-endurance-racer/thumbnail.png", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), 150, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"), "INP002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!", "Intermediate", 180, true, new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), "Mad Hornet Airplane", "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}", "mad-hornet-airplane", "instock-products/mad-hornet-airplane/thumbnail.png", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), 200, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), "INP003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!", "Advanced", 240, true, new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"), "Eagle 3D Puzzle", "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}", "eagle-3d-puzzle", "instock-products/ugt-24-endurance-racer/thumbnail.png", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), 180, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4"), "INP004", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!", "Advanced", 300, true, new Guid("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4"), "Sports Car 3D Puzzle", "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}", "sports-car-3d-puzzle", "instock-products/mad-hornet-airplane/thumbnail.png", new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), 250, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("b5b5b5b5-b5b5-b5b5-b5b5-b5b5b5b5b5b5"), "INP005", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!", "Intermediate", 200, true, new Guid("d5d5d5d5-d5d5-d5d5-d5d5-d5d5d5d5d5d5"), "Airplane 3D Puzzle", "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}", "airplane-3d-puzzle", "instock-products/ugt-24-endurance-racer/thumbnail.png", new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), 220, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), "INP006", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!", "Intermediate", 150, true, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), "Motorcycle 3D Puzzle", "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}", "motorcycle-3d-puzzle", "instock-products/ugt-24-endurance-racer/thumbnail.png", new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), 180, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"), "INP007", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!", "Basic", 130, true, new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), "Tiger 3D Puzzle", "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}", "tiger-3d-puzzle", "instock-products/mad-hornet-airplane/thumbnail.png", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), 170, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), "INP008", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!", "Basic", 100, true, new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"), "Dolphin 3D Puzzle", "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}", "dolphin-3d-puzzle", "instock-products/mad-hornet-airplane/thumbnail.png", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), 130, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"), "INP009", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!", "Intermediate", 170, true, new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), "Helicopter 3D Puzzle", "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}", "helicopter-3d-puzzle", "instock-products/ugt-24-endurance-racer/thumbnail.png", new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), 190, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000010"), new Guid("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4"), "INP010", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!", "Advanced", 360, true, new Guid("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4"), "Dragon 3D Puzzle", "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}", "dragon-3d-puzzle", "instock-products/mad-hornet-airplane/thumbnail.png", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), 300, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "cart",
                table: "in_stock_product_variant_replicas",
                columns: new[] { "id", "assembled_height_mm", "assembled_length_mm", "assembled_width_mm", "color", "created_at", "in_stock_product_id", "is_active", "sku", "updated_at" },
                values: new object[,]
                {
                    { new Guid("20000000-0001-0000-0000-000000000001"), 261, 218, 159, "Red", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, "SKU001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0001-0000-0000-000000000002"), 297, 174, 138, "Blue", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, "SKU002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0002-0000-0000-000000000001"), 175, 229, 263, "Green", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, "SKU003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0003-0000-0000-000000000001"), 224, 131, 171, "Yellow", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, "SKU004", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0003-0000-0000-000000000002"), 289, 182, 292, "Black", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, "SKU005", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0004-0000-0000-000000000001"), 158, 156, 240, "White", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, "SKU006", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0004-0000-0000-000000000002"), 127, 225, 187, "Orange", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, "SKU007", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0005-0000-0000-000000000001"), 222, 219, 201, "Purple", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, "SKU008", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0006-0000-0000-000000000001"), 227, 154, 278, "Red", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, "SKU009", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0006-0000-0000-000000000002"), 152, 288, 296, "Blue", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, "SKU010", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0007-0000-0000-000000000001"), 177, 221, 207, "Green", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), true, "SKU011", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0008-0000-0000-000000000001"), 297, 137, 135, "Yellow", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, "SKU012", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0008-0000-0000-000000000002"), 274, 195, 221, "Black", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, "SKU013", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0009-0000-0000-000000000001"), 102, 146, 111, "White", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), true, "SKU014", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0010-0000-0000-000000000001"), 154, 265, 262, "Orange", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, "SKU015", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0010-0000-0000-000000000002"), 251, 150, 186, "Purple", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, "SKU016", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
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
