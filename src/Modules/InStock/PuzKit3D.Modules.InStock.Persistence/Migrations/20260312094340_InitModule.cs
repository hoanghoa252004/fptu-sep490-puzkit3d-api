using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PuzKit3D.Modules.InStock.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "instock");

            migrationBuilder.CreateTable(
                name: "assembly_method_replicas",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assembly_method_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "capability_replicas",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_capability_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "instock_orders",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    customer_phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    customer_email = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    customer_province_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    customer_province_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    customer_district_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    customer_district_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    customer_ward_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    customer_ward_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    sub_total_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    shipping_fee = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    used_coin_amount = table.Column<int>(type: "integer", nullable: false),
                    used_coin_amount_as_money = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    grand_total_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    payment_method = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    is_paid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instock_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "instock_prices",
                schema: "instock",
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
                    table.PrimaryKey("pk_instock_prices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "instock_products",
                schema: "instock",
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
                    capability_id = table.Column<Guid>(type: "uuid", nullable: false),
                    material_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instock_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "material_replicas",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_material_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "topic_replicas",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_topic_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "instock_product_capability_detail",
                schema: "instock",
                columns: table => new
                {
                    instock_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    capability_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instock_product_capability_detail", x => new { x.instock_product_id, x.capability_id });
                    table.ForeignKey(
                        name: "FK__instock_product__instock_product_capability_detail",
                        column: x => x.instock_product_id,
                        principalSchema: "instock",
                        principalTable: "instock_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "instock_product_variants",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    instock_product_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("pk_instock_product_variants", x => x.id);
                    table.ForeignKey(
                        name: "FK__instock_product__instock_product_variant",
                        column: x => x.instock_product_id,
                        principalSchema: "instock",
                        principalTable: "instock_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "parts",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    part_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    instock_product_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parts", x => x.id);
                    table.ForeignKey(
                        name: "FK__instock_product__part",
                        column: x => x.instock_product_id,
                        principalSchema: "instock",
                        principalTable: "instock_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "instock_inventories",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    instock_product_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_quantity = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instock_inventories", x => x.id);
                    table.ForeignKey(
                        name: "FK__instock_product_variant__instock_inventory",
                        column: x => x.instock_product_variant_id,
                        principalSchema: "instock",
                        principalTable: "instock_product_variants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "instock_product_price_details",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    instock_price_id = table.Column<Guid>(type: "uuid", nullable: false),
                    instock_product_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instock_product_price_details", x => x.id);
                    table.ForeignKey(
                        name: "FK__instock_price__instock_product_price_detail",
                        column: x => x.instock_price_id,
                        principalSchema: "instock",
                        principalTable: "instock_prices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__instock_product_variant__instock_product_price_detail",
                        column: x => x.instock_product_variant_id,
                        principalSchema: "instock",
                        principalTable: "instock_product_variants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pieces",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    part_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pieces", x => x.id);
                    table.ForeignKey(
                        name: "FK__part__piece",
                        column: x => x.part_id,
                        principalSchema: "instock",
                        principalTable: "parts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "instock_order_details",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    instock_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    instock_product_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sku = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    product_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    variant_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    unit_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    instock_product_price_detail_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instock_order_details", x => x.id);
                    table.ForeignKey(
                        name: "FK__instock_order__instock_order_detail",
                        column: x => x.instock_order_id,
                        principalSchema: "instock",
                        principalTable: "instock_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__instock_order_detail__instock_product_price_detail",
                        column: x => x.instock_product_price_detail_id,
                        principalSchema: "instock",
                        principalTable: "instock_product_price_details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__instock_order_detail__instock_product_variant",
                        column: x => x.instock_product_variant_id,
                        principalSchema: "instock",
                        principalTable: "instock_product_variants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "assembly_method_replicas",
                columns: new[] { "id", "created_at", "description", "is_active", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Easy snap assembly", true, "Snap-Fit", "snap-fit", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Requires glue assembly", true, "Glue-Based", "glue-based", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "capability_replicas",
                columns: new[] { "id", "created_at", "description", "is_active", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Suitable for beginners", true, "Beginner Friendly", "beginner-friendly", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "For advanced builders", true, "Advanced Building", "advanced-building", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "instock_prices",
                columns: new[] { "id", "created_at", "effective_from", "effective_to", "is_active", "name", "priority", "updated_at" },
                values: new object[,]
                {
                    { new Guid("99999999-9999-9999-9999-999999999991"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2099, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), true, "Standard", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("99999999-9999-9999-9999-999999999992"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 23, 59, 59, 0, DateTimeKind.Utc), true, "Sale Summer Vacation", 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "instock_products",
                columns: new[] { "id", "assembly_method_id", "capability_id", "code", "created_at", "description", "difficult_level", "estimated_build_time", "is_active", "material_id", "name", "preview_asset", "slug", "thumbnail_url", "topic_id", "total_piece_count", "updated_at" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Beautiful lion 3D puzzle", "Basic", 120, true, new Guid("55555555-5555-5555-5555-555555555555"), "Lion 3D Puzzle", "{\"main\":\"https://example.com/lion-preview.jpg\"}", "lion-3d-puzzle", "https://example.com/lion.jpg", new Guid("11111111-1111-1111-1111-111111111111"), 150, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Majestic elephant 3D puzzle", "Intermediate", 180, true, new Guid("55555555-5555-5555-5555-555555555555"), "Elephant 3D Puzzle", "{\"main\":\"https://example.com/elephant-preview.jpg\"}", "elephant-3d-puzzle", "https://example.com/elephant.jpg", new Guid("11111111-1111-1111-1111-111111111111"), 200, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("88888888-8888-8888-8888-888888888888"), "INP003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Soaring eagle 3D puzzle", "Advanced", 240, true, new Guid("55555555-5555-5555-5555-555555555555"), "Eagle 3D Puzzle", "{\"main\":\"https://example.com/eagle-preview.jpg\"}", "eagle-3d-puzzle", "https://example.com/eagle.jpg", new Guid("11111111-1111-1111-1111-111111111111"), 180, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("88888888-8888-8888-8888-888888888888"), "INP004", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sleek sports car 3D puzzle", "Advanced", 300, true, new Guid("66666666-6666-6666-6666-666666666666"), "Sports Car 3D Puzzle", "{\"main\":\"https://example.com/sports-car-preview.jpg\"}", "sports-car-3d-puzzle", "https://example.com/sports-car.jpg", new Guid("22222222-2222-2222-2222-222222222222"), 250, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP005", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Flying airplane 3D puzzle", "Intermediate", 200, true, new Guid("66666666-6666-6666-6666-666666666666"), "Airplane 3D Puzzle", "{\"main\":\"https://example.com/airplane-preview.jpg\"}", "airplane-3d-puzzle", "https://example.com/airplane.jpg", new Guid("22222222-2222-2222-2222-222222222222"), 220, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP006", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cool motorcycle 3D puzzle", "Intermediate", 150, true, new Guid("66666666-6666-6666-6666-666666666666"), "Motorcycle 3D Puzzle", "{\"main\":\"https://example.com/motorcycle-preview.jpg\"}", "motorcycle-3d-puzzle", "https://example.com/motorcycle.jpg", new Guid("22222222-2222-2222-2222-222222222222"), 180, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP007", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fierce tiger 3D puzzle", "Basic", 130, true, new Guid("55555555-5555-5555-5555-555555555555"), "Tiger 3D Puzzle", "{\"main\":\"https://example.com/tiger-preview.jpg\"}", "tiger-3d-puzzle", "https://example.com/tiger.jpg", new Guid("11111111-1111-1111-1111-111111111111"), 170, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP008", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Playful dolphin 3D puzzle", "Basic", 100, true, new Guid("55555555-5555-5555-5555-555555555555"), "Dolphin 3D Puzzle", "{\"main\":\"https://example.com/dolphin-preview.jpg\"}", "dolphin-3d-puzzle", "https://example.com/dolphin.jpg", new Guid("11111111-1111-1111-1111-111111111111"), 130, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP009", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Flying helicopter 3D puzzle", "Intermediate", 170, true, new Guid("66666666-6666-6666-6666-666666666666"), "Helicopter 3D Puzzle", "{\"main\":\"https://example.com/helicopter-preview.jpg\"}", "helicopter-3d-puzzle", "https://example.com/helicopter.jpg", new Guid("22222222-2222-2222-2222-222222222222"), 190, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000010"), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("88888888-8888-8888-8888-888888888888"), "INP010", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mythical dragon 3D puzzle", "Advanced", 360, true, new Guid("55555555-5555-5555-5555-555555555555"), "Dragon 3D Puzzle", "{\"main\":\"https://example.com/dragon-preview.jpg\"}", "dragon-3d-puzzle", "https://example.com/dragon.jpg", new Guid("11111111-1111-1111-1111-111111111111"), 300, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "material_replicas",
                columns: new[] { "id", "created_at", "description", "is_active", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Natural wood material", true, "Wood", "wood", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Durable plastic material", true, "Plastic", "plastic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "topic_replicas",
                columns: new[] { "id", "created_at", "description", "is_active", "name", "parent_id", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Animal themed puzzles", true, "Animals", null, "animals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vehicle themed puzzles", true, "Vehicles", null, "vehicles", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "instock_product_variants",
                columns: new[] { "id", "assembled_height_mm", "assembled_length_mm", "assembled_width_mm", "color", "created_at", "instock_product_id", "is_active", "sku", "updated_at" },
                values: new object[,]
                {
                    { new Guid("018febcc-7408-4ab0-98af-d94eca6c88d5"), 123, 111, 247, "White", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), true, "SKU014", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("28cb78ff-5a92-450f-af8f-8b4398ef2020"), 141, 118, 283, "Purple", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, "SKU008", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("477da0df-065b-4a2b-81ba-606e8729c0a0"), 255, 131, 104, "Black", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, "SKU005", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("67443f79-00ea-4788-860f-022dee5bd2af"), 248, 296, 281, "Red", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, "SKU001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("6f9812f6-07b7-4fcb-8995-2f45c9f05e81"), 198, 286, 277, "Red", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, "SKU009", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("750c5e07-f54d-4afe-b43e-f9b605cfdf3b"), 248, 292, 125, "Yellow", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, "SKU012", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("751d2130-f6c0-4134-af81-395b12ef2867"), 151, 269, 213, "Yellow", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, "SKU004", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("9d7256d9-b479-4622-b91c-f0702b08eee1"), 137, 281, 199, "Green", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, "SKU003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a9517a9f-bc7c-47d4-861b-a6efba6ff82b"), 102, 240, 130, "Green", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), true, "SKU011", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("bfd171ed-2f2a-4206-848f-90806ff04d19"), 225, 172, 123, "Black", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, "SKU013", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d3ac62a5-f9f3-4586-8ec8-6ee09fda5238"), 251, 219, 270, "Orange", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, "SKU007", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e32a9a75-b546-46c3-859b-809045e50e5b"), 217, 154, 206, "Purple", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, "SKU016", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e394c8e7-5acb-404c-aeca-340aa9c985e3"), 176, 122, 136, "Blue", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, "SKU010", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e81cb4ad-645b-493b-9374-904bbe693cf8"), 269, 285, 172, "Orange", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, "SKU015", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("ec3c3da2-0955-40c3-bd2c-8b56eb9f2810"), 279, 227, 237, "White", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, "SKU006", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("fda00513-a823-43c4-b90c-102b07faf865"), 129, 109, 103, "Blue", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, "SKU002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "instock_inventories",
                columns: new[] { "id", "created_at", "instock_product_variant_id", "total_quantity", "updated_at" },
                values: new object[,]
                {
                    { new Guid("062bec5a-c354-45e9-b7a7-4073efcaaf9f"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9d7256d9-b479-4622-b91c-f0702b08eee1"), 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("08b64637-e08f-4b6e-801d-50856cc374db"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("28cb78ff-5a92-450f-af8f-8b4398ef2020"), 160, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("2933754b-87e0-4bac-9334-d49410e2268e"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("e394c8e7-5acb-404c-aeca-340aa9c985e3"), 96, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("2e2cbf6a-b2a8-469b-84db-24de7ebbcb2b"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("6f9812f6-07b7-4fcb-8995-2f45c9f05e81"), 152, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33200c61-b54c-415f-beba-6d1c42386b2d"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("477da0df-065b-4a2b-81ba-606e8729c0a0"), 151, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("3e57feb2-74bb-481e-820f-edba1b37f6e9"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a9517a9f-bc7c-47d4-861b-a6efba6ff82b"), 165, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("58220535-16ab-4775-8c2e-5956dfab8fb9"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("67443f79-00ea-4788-860f-022dee5bd2af"), 148, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("85c5355f-177a-493e-a669-036e5db3d728"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("750c5e07-f54d-4afe-b43e-f9b605cfdf3b"), 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("86372cfc-4445-4699-a32d-bc899715ddf0"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("fda00513-a823-43c4-b90c-102b07faf865"), 126, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("97d216cd-14ba-41ab-9da1-1c9cb6ab48a8"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("e81cb4ad-645b-493b-9374-904bbe693cf8"), 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("bbc46b2d-ae0a-482e-b4ec-89f22fe7a2e6"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("018febcc-7408-4ab0-98af-d94eca6c88d5"), 198, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d9950d8e-dc5c-40a3-b985-4c16acf136c8"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("ec3c3da2-0955-40c3-bd2c-8b56eb9f2810"), 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("dea71ea0-dc53-4a77-84cb-0a40ce7b424e"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("751d2130-f6c0-4134-af81-395b12ef2867"), 189, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e4789c5d-6100-4ded-9b6d-fd2b7814d15a"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d3ac62a5-f9f3-4586-8ec8-6ee09fda5238"), 118, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f7fb7beb-64e9-4894-b5c2-b5efad26ee16"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("e32a9a75-b546-46c3-859b-809045e50e5b"), 128, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("fd286d17-5822-4ada-81e0-b5305e46af2d"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("bfd171ed-2f2a-4206-848f-90806ff04d19"), 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "instock_product_price_details",
                columns: new[] { "id", "created_at", "instock_price_id", "instock_product_variant_id", "is_active", "unit_price", "updated_at" },
                values: new object[,]
                {
                    { new Guid("0ec11ea2-b783-4204-938b-a081a41d5f64"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("018febcc-7408-4ab0-98af-d94eca6c88d5"), true, 883000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("2b6ac539-c194-4c07-b6fe-0ff72e921ece"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("6f9812f6-07b7-4fcb-8995-2f45c9f05e81"), true, 218400.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("2cfc4d5a-4d8d-494e-bf50-bcfd912173e8"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("28cb78ff-5a92-450f-af8f-8b4398ef2020"), true, 466000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("2f140f91-5d92-452d-a3d9-a8ae06a1e41a"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("fda00513-a823-43c4-b90c-102b07faf865"), true, 275000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("500c49a9-10cb-403b-8193-7a36020e2815"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("750c5e07-f54d-4afe-b43e-f9b605cfdf3b"), true, 151000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("5772802f-0ace-4c3a-9f1f-dfa6af3b0bad"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("018febcc-7408-4ab0-98af-d94eca6c88d5"), true, 706400.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("5c72cc41-4bfb-42bd-bc44-45b06f4ebde7"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("bfd171ed-2f2a-4206-848f-90806ff04d19"), true, 118400.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("5f9db524-5820-4320-bf8b-b2620233d4c3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("ec3c3da2-0955-40c3-bd2c-8b56eb9f2810"), true, 455200.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("629b82dc-db84-46a3-b143-f1cc40039807"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("6f9812f6-07b7-4fcb-8995-2f45c9f05e81"), true, 273000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("707d3607-0625-49de-9191-09a2464adeb2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("e394c8e7-5acb-404c-aeca-340aa9c985e3"), true, 196800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("743d924a-1a82-4686-9c68-82aa1fdbc012"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("750c5e07-f54d-4afe-b43e-f9b605cfdf3b"), true, 120800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("867da5f3-d856-4f52-9372-796ae33b35d1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("e81cb4ad-645b-493b-9374-904bbe693cf8"), true, 288000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("8bf4ffad-1a2b-4913-b6f4-ac58d01b8096"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("bfd171ed-2f2a-4206-848f-90806ff04d19"), true, 148000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("97069190-1fe8-4404-8ad4-5407aaa9c84b"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("67443f79-00ea-4788-860f-022dee5bd2af"), true, 831000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("9b78eaca-e085-4ea0-993f-bbf5190f020e"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("ec3c3da2-0955-40c3-bd2c-8b56eb9f2810"), true, 569000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("9ca3fc82-b2af-4f9c-92a3-5b08bdab0ff0"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("a9517a9f-bc7c-47d4-861b-a6efba6ff82b"), true, 592000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("9cd2d090-8d77-4339-a58e-154c58180eb7"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("e32a9a75-b546-46c3-859b-809045e50e5b"), true, 256000.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b83544d1-f3f1-42b3-acdb-56c03cfc3df1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("751d2130-f6c0-4134-af81-395b12ef2867"), true, 106000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("bc32b47d-3de2-410c-9452-4fb80d9248bb"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("fda00513-a823-43c4-b90c-102b07faf865"), true, 220000.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("bd46f7ae-7bd6-4b08-9061-fe959156d1fd"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("751d2130-f6c0-4134-af81-395b12ef2867"), true, 84800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c81970db-2f07-4df4-9303-16cdc4147203"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("477da0df-065b-4a2b-81ba-606e8729c0a0"), true, 101000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("ce6ec16e-be3f-4b1c-853e-afca7cf24605"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("e394c8e7-5acb-404c-aeca-340aa9c985e3"), true, 246000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d3887891-c169-47e7-9221-216cdeca0723"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("28cb78ff-5a92-450f-af8f-8b4398ef2020"), true, 372800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d4da5380-cf58-4f6f-84af-3af72a2555ec"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("e32a9a75-b546-46c3-859b-809045e50e5b"), true, 320000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("de1bed4f-5119-499f-9ad9-acd2e430e1a3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("477da0df-065b-4a2b-81ba-606e8729c0a0"), true, 80800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("de96a6f1-de83-462c-b45a-0297ebeb7ccd"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("67443f79-00ea-4788-860f-022dee5bd2af"), true, 664800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("df725abf-d031-4aab-93c8-34479f1c120c"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("d3ac62a5-f9f3-4586-8ec8-6ee09fda5238"), true, 132000.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e1df6d77-af71-4c0e-a449-58cfd21c3e77"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("9d7256d9-b479-4622-b91c-f0702b08eee1"), true, 412000.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e35c7b1a-3882-4d16-880e-809eb7120647"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("a9517a9f-bc7c-47d4-861b-a6efba6ff82b"), true, 473600.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e6e5ac51-227f-448f-ab58-46a77dc922a3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("e81cb4ad-645b-493b-9374-904bbe693cf8"), true, 230400.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("eeb9f290-0d2e-436e-baa2-0e3fe3c94f60"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("9d7256d9-b479-4622-b91c-f0702b08eee1"), true, 515000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f35f99e0-dc76-4742-adf9-02463fb36f1f"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("d3ac62a5-f9f3-4586-8ec8-6ee09fda5238"), true, 165000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "UK__assembly_method_replica__slug",
                schema: "instock",
                table: "assembly_method_replicas",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__capability_replica__slug",
                schema: "instock",
                table: "capability_replicas",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_instock_inventories_instock_product_variant_id",
                schema: "instock",
                table: "instock_inventories",
                column: "instock_product_variant_id");

            migrationBuilder.CreateIndex(
                name: "CUK___instock_order_detail___instock_order_id__instock_product_variant_id",
                schema: "instock",
                table: "instock_order_details",
                columns: new[] { "instock_order_id", "instock_product_variant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_instock_order_details_instock_product_price_detail_id",
                schema: "instock",
                table: "instock_order_details",
                column: "instock_product_price_detail_id");

            migrationBuilder.CreateIndex(
                name: "ix_instock_order_details_instock_product_variant_id",
                schema: "instock",
                table: "instock_order_details",
                column: "instock_product_variant_id");

            migrationBuilder.CreateIndex(
                name: "UK__instock_order__code",
                schema: "instock",
                table: "instock_orders",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "CUK___instock_product_price_detail___instock_price_id__instock_product_variant_id",
                schema: "instock",
                table: "instock_product_price_details",
                columns: new[] { "instock_price_id", "instock_product_variant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_instock_product_price_details_instock_product_variant_id",
                schema: "instock",
                table: "instock_product_price_details",
                column: "instock_product_variant_id");

            migrationBuilder.CreateIndex(
                name: "ix_instock_product_variants_instock_product_id",
                schema: "instock",
                table: "instock_product_variants",
                column: "instock_product_id");

            migrationBuilder.CreateIndex(
                name: "UK__instock_product_variant__sku",
                schema: "instock",
                table: "instock_product_variants",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__instock_product__slug",
                schema: "instock",
                table: "instock_products",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__material_replica__slug",
                schema: "instock",
                table: "material_replicas",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_parts_instock_product_id",
                schema: "instock",
                table: "parts",
                column: "instock_product_id");

            migrationBuilder.CreateIndex(
                name: "ix_pieces_part_id",
                schema: "instock",
                table: "pieces",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "UK__topic_replica__slug",
                schema: "instock",
                table: "topic_replicas",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assembly_method_replicas",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "capability_replicas",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_inventories",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_order_details",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_product_capability_detail",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "material_replicas",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "pieces",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "topic_replicas",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_orders",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_product_price_details",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "parts",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_prices",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_product_variants",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_products",
                schema: "instock");
        }
    }
}
