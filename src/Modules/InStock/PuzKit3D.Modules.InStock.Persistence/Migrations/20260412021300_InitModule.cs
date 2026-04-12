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
                    factor_percentage = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
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
                    factor_percentage = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_capability_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "drive_replicas",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    min_volume = table.Column<int>(type: "integer", nullable: true),
                    quantity_in_stock = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drive_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "instock_order_configs",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_must_complete_in_days = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instock_order_configs", x => x.id);
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
                    customer_province_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    customer_district_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    customer_ward_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    detail_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sub_total_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    shipping_fee = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    used_coin_amount = table.Column<int>(type: "integer", nullable: false),
                    grand_total_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    payment_method = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    is_paid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    must_complete_before = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    factor_percentage = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    base_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_material_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "support_ticket_replicas",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    proof = table.Column<string>(type: "varchar(500)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_support_ticket_replicas", x => x.id);
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
                    factor_percentage = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
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
                        name: "fk_instock_product_capability_detail_instock_products_instock_",
                        column: x => x.instock_product_id,
                        principalSchema: "instock",
                        principalTable: "instock_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "instock_product_drives",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    instock_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    drive_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instock_product_drives", x => x.id);
                    table.ForeignKey(
                        name: "fk_instock_product_drives_instock_products_instock_product_id",
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
                name: "support_ticket_detail_replicas",
                schema: "instock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    support_ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    part_id = table.Column<Guid>(type: "uuid", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_support_ticket_detail_replicas", x => x.id);
                    table.ForeignKey(
                        name: "fk_support_ticket_detail_replicas_support_ticket_replicas_supp",
                        column: x => x.support_ticket_id,
                        principalSchema: "instock",
                        principalTable: "support_ticket_replicas",
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
                columns: new[] { "id", "created_at", "description", "factor_percentage", "is_active", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly using tight fitting pieces", 1.15m, true, "Friction Fit", "friction-fit", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b5b5b5b5-b5b5-b5b5-b5b5-b5b5b5b5b5b5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly using magnetic connections", 1.4m, true, "Magnetic Assembly", "magnetic-assembly", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Easy snap assembly without tools", 1.1m, true, "Snap-Fit", "snap-fit", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly using adhesive bonding", 1.0m, true, "Glue Assembly", "glue-assembly", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly using screws and bolts", 1.3m, true, "Screw Assembly", "screw-assembly", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "capability_replicas",
                columns: new[] { "id", "created_at", "description", "factor_percentage", "is_active", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Model operated by manual movement", 1.3m, true, "Manual Movement", "manual-movement", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Model with LED lighting effects", 1.4m, true, "LED Light Feature", "led-light-feature", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Model with musical features via rotating mechanism", 1.6m, true, "Musical Gear", "musical-gear", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Static model for display only", 1.0m, true, "Static Display", "static-display", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Model with electric motor-powered movement", 1.8m, true, "Move with Motor", "move-with-motor", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "drive_replicas",
                columns: new[] { "id", "created_at", "description", "is_active", "min_volume", "name", "quantity_in_stock", "updated_at" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Router module for rotate", true, 5, "Router", 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Electric motor drive", true, 50, "Motor", 100, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mechanical gear system", true, 30, "Gearbox", 150, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LED lighting system", true, 10, "LED Module", 200, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mechanical music box", true, 50, "Music Box", 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "instock_order_configs",
                columns: new[] { "id", "order_must_complete_in_days", "updated_at" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), 7, new DateTime(2026, 3, 30, 0, 0, 0, 0, DateTimeKind.Utc) });

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
                columns: new[] { "id", "assembly_method_id", "code", "created_at", "description", "difficult_level", "estimated_build_time", "is_active", "material_id", "name", "preview_asset", "slug", "thumbnail_url", "topic_id", "total_piece_count", "updated_at" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), "INP001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!", "Basic", 120, true, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), "UGT-24 Endurance Racer", "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}", "ugt-24-endurance-racer", "instock-products/ugt-24-endurance-racer/thumbnail.png", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), 150, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"), "INP002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!", "Intermediate", 180, true, new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), "Mad Hornet Airplane", "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}", "mad-hornet-airplane", "instock-products/mad-hornet-airplane/thumbnail.png", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), 200, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), "INP003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!", "Advanced", 240, true, new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"), "Eagle 3D Puzzle", "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}", "eagle-3d-puzzle", "instock-products/ugt-24-endurance-racer/thumbnail.png", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), 180, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4"), "INP004", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!", "Advanced", 300, true, new Guid("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4"), "Sports Car 3D Puzzle", "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}", "sports-car-3d-puzzle", "instock-products/mad-hornet-airplane/thumbnail.png", new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), 250, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("b5b5b5b5-b5b5-b5b5-b5b5-b5b5b5b5b5b5"), "INP005", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!", "Intermediate", 200, true, new Guid("d5d5d5d5-d5d5-d5d5-d5d5-d5d5d5d5d5d5"), "Airplane 3D Puzzle", "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}", "airplane-3d-puzzle", "instock-products/ugt-24-endurance-racer/thumbnail.png", new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), 220, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), "INP006", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!", "Intermediate", 150, true, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), "Motorcycle 3D Puzzle", "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}", "motorcycle-3d-puzzle", "instock-products/ugt-24-endurance-racer/thumbnail.png", new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"), 180, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"), "INP007", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!", "Basic", 130, true, new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), "Tiger 3D Puzzle", "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}", "tiger-3d-puzzle", "instock-products/mad-hornet-airplane/thumbnail.png", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), 170, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), "INP008", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!", "Basic", 100, true, new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"), "Dolphin 3D Puzzle", "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}", "dolphin-3d-puzzle", "instock-products/mad-hornet-airplane/thumbnail.png", new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"), 130, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4"), "INP009", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!", "Intermediate", 170, true, new Guid("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4"), "Helicopter 3D Puzzle", "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}", "helicopter-3d-puzzle", "instock-products/ugt-24-endurance-racer/thumbnail.png", new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), 190, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000010"), new Guid("b5b5b5b5-b5b5-b5b5-b5b5-b5b5b5b5b5b5"), "INP010", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!", "Advanced", 360, true, new Guid("d5d5d5d5-d5d5-d5d5-d5d5-d5d5d5d5d5d5"), "Dragon 3D Puzzle", "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}", "dragon-3d-puzzle", "instock-products/mad-hornet-airplane/thumbnail.png", new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"), 300, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "material_replicas",
                columns: new[] { "id", "base_price", "created_at", "description", "factor_percentage", "is_active", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), 2000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Durable plastic material", 0.85m, true, "Plastic", "plastic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"), 9000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Premium metal components", 1.6m, true, "Metal", "metal", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4"), 1000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Eco-friendly cardboard material", 0.7m, true, "Cardboard", "cardboard", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d5d5d5d5-d5d5-d5d5-d5d5-d5d5d5d5d5d5"), 7000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Advanced composite materials", 1.3m, true, "Composite", "composite", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), 4000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Natural wood material", 1.0m, true, "Wood", "wood", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "topic_replicas",
                columns: new[] { "id", "created_at", "description", "factor_percentage", "is_active", "name", "parent_id", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Animal themed 3D puzzles", 1.1m, true, "Animals", null, "animals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vehicle and transportation themed puzzles", 1.3m, true, "Vehicles", null, "vehicles", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Famous buildings and landmarks", 1.5m, true, "Architecture", null, "architecture", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Natural landscapes and scenery", 1.0m, true, "Nature", null, "nature", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fantasy creatures and magical worlds", 1.6m, true, "Fantasy", null, "fantasy", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "instock_product_capability_detail",
                columns: new[] { "capability_id", "instock_product_id" },
                values: new object[,]
                {
                    { new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"), new Guid("10000000-0000-0000-0000-000000000002") },
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), new Guid("10000000-0000-0000-0000-000000000002") },
                    { new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4"), new Guid("10000000-0000-0000-0000-000000000004") },
                    { new Guid("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5"), new Guid("10000000-0000-0000-0000-000000000004") },
                    { new Guid("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5"), new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), new Guid("10000000-0000-0000-0000-000000000006") },
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), new Guid("10000000-0000-0000-0000-000000000006") },
                    { new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"), new Guid("10000000-0000-0000-0000-000000000007") },
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), new Guid("10000000-0000-0000-0000-000000000007") },
                    { new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"), new Guid("10000000-0000-0000-0000-000000000008") },
                    { new Guid("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4"), new Guid("10000000-0000-0000-0000-000000000008") },
                    { new Guid("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4"), new Guid("10000000-0000-0000-0000-000000000009") },
                    { new Guid("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5"), new Guid("10000000-0000-0000-0000-000000000009") },
                    { new Guid("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5"), new Guid("10000000-0000-0000-0000-000000000010") },
                    { new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), new Guid("10000000-0000-0000-0000-000000000010") }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "instock_product_drives",
                columns: new[] { "id", "drive_id", "instock_product_id", "quantity" },
                values: new object[,]
                {
                    { new Guid("127f82f8-043b-4c17-87a6-6c3042b946c8"), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("10000000-0000-0000-0000-000000000006"), 1 },
                    { new Guid("2a714d62-3e8f-46f0-896f-d9183b5ec038"), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("10000000-0000-0000-0000-000000000001"), 1 },
                    { new Guid("2bcbb9f2-9e39-445a-b790-6a54852849de"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000006"), 1 },
                    { new Guid("2c404700-78a6-48f1-ac4f-5589cda2a331"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("10000000-0000-0000-0000-000000000005"), 3 },
                    { new Guid("2f2a0878-358d-455e-a803-5d8b9178b63d"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("10000000-0000-0000-0000-000000000004"), 2 },
                    { new Guid("353365b6-74c5-4afd-a2c1-b8f2890d663b"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("10000000-0000-0000-0000-000000000003"), 2 },
                    { new Guid("37ff4975-1b3e-4151-a20a-d44698db01a4"), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("10000000-0000-0000-0000-000000000007"), 1 },
                    { new Guid("38273c1f-cd96-47de-b9f9-65df8b483fa9"), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("10000000-0000-0000-0000-000000000010"), 1 },
                    { new Guid("5b79617a-f481-4de4-b22b-f2295d975f5b"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("10000000-0000-0000-0000-000000000008"), 2 },
                    { new Guid("6398e90a-8473-4deb-bb8e-c837cfc995c8"), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("10000000-0000-0000-0000-000000000004"), 2 },
                    { new Guid("6d54f48f-05f4-44bc-88cd-cf9dfb751a23"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("10000000-0000-0000-0000-000000000001"), 2 },
                    { new Guid("6e03fa7c-766e-4af3-9848-b60c26510ae6"), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("10000000-0000-0000-0000-000000000002"), 1 },
                    { new Guid("7872dd2f-6552-42fa-a109-762e0a977587"), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("10000000-0000-0000-0000-000000000005"), 1 },
                    { new Guid("78826ed5-4058-4239-8a90-1e768f5aa244"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000010"), 1 },
                    { new Guid("792bb0cd-adaf-4a1a-9fb7-fa55d392424d"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000005"), 1 },
                    { new Guid("7f72e24b-69a8-4bf4-b3c1-7b74488bcf62"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("10000000-0000-0000-0000-000000000002"), 3 },
                    { new Guid("82349e52-3768-4249-87ed-906e54658a62"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("10000000-0000-0000-0000-000000000007"), 2 },
                    { new Guid("8b4aaac2-4715-499f-a40e-17b68435d902"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000001"), 1 },
                    { new Guid("9359614f-c609-47ae-93de-5f8687108d32"), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("10000000-0000-0000-0000-000000000004"), 1 },
                    { new Guid("acd25992-e9b2-4811-a40d-0871e5b9d938"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("10000000-0000-0000-0000-000000000009"), 2 },
                    { new Guid("b13ce521-6feb-44a4-8853-af21aaf1b724"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("10000000-0000-0000-0000-000000000006"), 2 },
                    { new Guid("cdf63565-d0e5-436d-8249-05b32059beeb"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("10000000-0000-0000-0000-000000000010"), 2 },
                    { new Guid("d18d54ea-2993-48e8-a671-7cba9fba5c94"), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("10000000-0000-0000-0000-000000000009"), 1 },
                    { new Guid("dccf852b-a411-4741-8cc9-99288de75b66"), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("10000000-0000-0000-0000-000000000003"), 1 },
                    { new Guid("f503de77-dfdb-4fce-9378-155ac1c7e73a"), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("10000000-0000-0000-0000-000000000009"), 1 },
                    { new Guid("f582d00a-c9c3-4d63-b9cd-c88e7c8e68b3"), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("10000000-0000-0000-0000-000000000008"), 1 }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "instock_product_variants",
                columns: new[] { "id", "assembled_height_mm", "assembled_length_mm", "assembled_width_mm", "color", "created_at", "instock_product_id", "is_active", "sku", "updated_at" },
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

            migrationBuilder.InsertData(
                schema: "instock",
                table: "instock_inventories",
                columns: new[] { "id", "created_at", "instock_product_variant_id", "total_quantity", "updated_at" },
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
                schema: "instock",
                table: "instock_product_price_details",
                columns: new[] { "id", "created_at", "instock_price_id", "instock_product_variant_id", "is_active", "unit_price", "updated_at" },
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
                name: "UK__instock_product_drive__product_drive",
                schema: "instock",
                table: "instock_product_drives",
                columns: new[] { "instock_product_id", "drive_id" },
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
                name: "ix_support_ticket_detail_replicas_order_item_id",
                schema: "instock",
                table: "support_ticket_detail_replicas",
                column: "order_item_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_ticket_detail_replicas_support_ticket_id",
                schema: "instock",
                table: "support_ticket_detail_replicas",
                column: "support_ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_ticket_replicas_order_id",
                schema: "instock",
                table: "support_ticket_replicas",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_ticket_replicas_status",
                schema: "instock",
                table: "support_ticket_replicas",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_support_ticket_replicas_user_id",
                schema: "instock",
                table: "support_ticket_replicas",
                column: "user_id");

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
                name: "drive_replicas",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_inventories",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_order_configs",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_order_details",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_product_capability_detail",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "instock_product_drives",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "material_replicas",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "support_ticket_detail_replicas",
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
                name: "support_ticket_replicas",
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
