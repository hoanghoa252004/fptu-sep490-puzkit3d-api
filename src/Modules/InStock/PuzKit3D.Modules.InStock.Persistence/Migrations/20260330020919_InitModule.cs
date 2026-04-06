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
                    part_type = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
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
                columns: new[] { "id", "created_at", "description", "is_active", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly using tight fitting pieces", true, "Friction Fit", "friction-fit", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b5b5b5b5-b5b5-b5b5-b5b5-b5b5b5b5b5b5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly using magnetic connections", true, "Magnetic Assembly", "magnetic-assembly", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Easy snap assembly without tools", true, "Snap-Fit", "snap-fit", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly using adhesive bonding", true, "Glue Assembly", "glue-assembly", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly using screws and bolts", true, "Screw Assembly", "screw-assembly", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "capability_replicas",
                columns: new[] { "id", "created_at", "description", "is_active", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Model operated by manual movement", true, "Manual Movement", "manual-movement", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Model with LED lighting effects", true, "LED Light Feature", "led-light-feature", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Model with musical features via rotating mechanism", true, "Musical Gear", "musical-gear", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Static model for display only", true, "Static Display", "static-display", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Model with electric motor-powered movement", true, "Move with Motor", "move-with-motor", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
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
                columns: new[] { "id", "created_at", "description", "is_active", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Durable plastic material", true, "Plastic", "plastic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Premium metal components", true, "Metal", "metal", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Eco-friendly cardboard material", true, "Cardboard", "cardboard", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d5d5d5d5-d5d5-d5d5-d5d5-d5d5d5d5d5d5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Advanced composite materials", true, "Composite", "composite", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Natural wood material", true, "Wood", "wood", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "instock",
                table: "topic_replicas",
                columns: new[] { "id", "created_at", "description", "is_active", "name", "parent_id", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Animal themed 3D puzzles", true, "Animals", null, "animals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vehicle and transportation themed puzzles", true, "Vehicles", null, "vehicles", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Famous buildings and landmarks", true, "Architecture", null, "architecture", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Natural landscapes and scenery", true, "Nature", null, "nature", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fantasy creatures and magical worlds", true, "Fantasy", null, "fantasy", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
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
                table: "parts",
                columns: new[] { "id", "code", "instock_product_id", "name", "part_type", "quantity" },
                values: new object[,]
                {
                    { new Guid("50000000-0001-0000-0000-000000000000"), "PAR0001", new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 1", "Structural", 10 },
                    { new Guid("50000000-0001-0001-0000-000000000000"), "PAR0002", new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 2", "Mechanical", 15 },
                    { new Guid("50000000-0001-0002-0000-000000000000"), "PAR0003", new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 3", "Decorative", 20 },
                    { new Guid("50000000-0001-0003-0000-000000000000"), "PAR0004", new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 4", "Structural", 25 },
                    { new Guid("50000000-0001-0004-0000-000000000000"), "PAR0005", new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 5", "Mechanical", 30 },
                    { new Guid("50000000-0001-0005-0000-000000000000"), "PAR0006", new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 6", "Decorative", 35 },
                    { new Guid("50000000-0001-0006-0000-000000000000"), "PAR0007", new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 7", "Structural", 40 },
                    { new Guid("50000000-0001-0007-0000-000000000000"), "PAR0008", new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 8", "Mechanical", 45 },
                    { new Guid("50000000-0001-0008-0000-000000000000"), "PAR0009", new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 9", "Decorative", 50 },
                    { new Guid("50000000-0001-0009-0000-000000000000"), "PAR0010", new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 10", "Structural", 55 },
                    { new Guid("50000000-0002-0000-0000-000000000000"), "PAR0011", new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 1", "Structural", 10 },
                    { new Guid("50000000-0002-0001-0000-000000000000"), "PAR0012", new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 2", "Mechanical", 15 },
                    { new Guid("50000000-0002-0002-0000-000000000000"), "PAR0013", new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 3", "Decorative", 20 },
                    { new Guid("50000000-0002-0003-0000-000000000000"), "PAR0014", new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 4", "Structural", 25 },
                    { new Guid("50000000-0002-0004-0000-000000000000"), "PAR0015", new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 5", "Mechanical", 30 },
                    { new Guid("50000000-0002-0005-0000-000000000000"), "PAR0016", new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 6", "Decorative", 35 },
                    { new Guid("50000000-0002-0006-0000-000000000000"), "PAR0017", new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 7", "Structural", 40 },
                    { new Guid("50000000-0002-0007-0000-000000000000"), "PAR0018", new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 8", "Mechanical", 45 },
                    { new Guid("50000000-0002-0008-0000-000000000000"), "PAR0019", new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 9", "Decorative", 50 },
                    { new Guid("50000000-0002-0009-0000-000000000000"), "PAR0020", new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 10", "Structural", 55 },
                    { new Guid("50000000-0003-0000-0000-000000000000"), "PAR0021", new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 1", "Structural", 10 },
                    { new Guid("50000000-0003-0001-0000-000000000000"), "PAR0022", new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 2", "Mechanical", 15 },
                    { new Guid("50000000-0003-0002-0000-000000000000"), "PAR0023", new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 3", "Decorative", 20 },
                    { new Guid("50000000-0003-0003-0000-000000000000"), "PAR0024", new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 4", "Structural", 25 },
                    { new Guid("50000000-0003-0004-0000-000000000000"), "PAR0025", new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 5", "Mechanical", 30 },
                    { new Guid("50000000-0003-0005-0000-000000000000"), "PAR0026", new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 6", "Decorative", 35 },
                    { new Guid("50000000-0003-0006-0000-000000000000"), "PAR0027", new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 7", "Structural", 40 },
                    { new Guid("50000000-0003-0007-0000-000000000000"), "PAR0028", new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 8", "Mechanical", 45 },
                    { new Guid("50000000-0003-0008-0000-000000000000"), "PAR0029", new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 9", "Decorative", 50 },
                    { new Guid("50000000-0003-0009-0000-000000000000"), "PAR0030", new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 10", "Structural", 55 },
                    { new Guid("50000000-0004-0000-0000-000000000000"), "PAR0031", new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 1", "Structural", 10 },
                    { new Guid("50000000-0004-0001-0000-000000000000"), "PAR0032", new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 2", "Mechanical", 15 },
                    { new Guid("50000000-0004-0002-0000-000000000000"), "PAR0033", new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 3", "Decorative", 20 },
                    { new Guid("50000000-0004-0003-0000-000000000000"), "PAR0034", new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 4", "Structural", 25 },
                    { new Guid("50000000-0004-0004-0000-000000000000"), "PAR0035", new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 5", "Mechanical", 30 },
                    { new Guid("50000000-0004-0005-0000-000000000000"), "PAR0036", new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 6", "Decorative", 35 },
                    { new Guid("50000000-0004-0006-0000-000000000000"), "PAR0037", new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 7", "Structural", 40 },
                    { new Guid("50000000-0004-0007-0000-000000000000"), "PAR0038", new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 8", "Mechanical", 45 },
                    { new Guid("50000000-0004-0008-0000-000000000000"), "PAR0039", new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 9", "Decorative", 50 },
                    { new Guid("50000000-0004-0009-0000-000000000000"), "PAR0040", new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 10", "Structural", 55 },
                    { new Guid("50000000-0005-0000-0000-000000000000"), "PAR0041", new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 1", "Structural", 10 },
                    { new Guid("50000000-0005-0001-0000-000000000000"), "PAR0042", new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 2", "Mechanical", 15 },
                    { new Guid("50000000-0005-0002-0000-000000000000"), "PAR0043", new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 3", "Decorative", 20 },
                    { new Guid("50000000-0005-0003-0000-000000000000"), "PAR0044", new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 4", "Structural", 25 },
                    { new Guid("50000000-0005-0004-0000-000000000000"), "PAR0045", new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 5", "Mechanical", 30 },
                    { new Guid("50000000-0005-0005-0000-000000000000"), "PAR0046", new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 6", "Decorative", 35 },
                    { new Guid("50000000-0005-0006-0000-000000000000"), "PAR0047", new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 7", "Structural", 40 },
                    { new Guid("50000000-0005-0007-0000-000000000000"), "PAR0048", new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 8", "Mechanical", 45 },
                    { new Guid("50000000-0005-0008-0000-000000000000"), "PAR0049", new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 9", "Decorative", 50 },
                    { new Guid("50000000-0005-0009-0000-000000000000"), "PAR0050", new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 10", "Structural", 55 },
                    { new Guid("50000000-0006-0000-0000-000000000000"), "PAR0051", new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 1", "Structural", 10 },
                    { new Guid("50000000-0006-0001-0000-000000000000"), "PAR0052", new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 2", "Mechanical", 15 },
                    { new Guid("50000000-0006-0002-0000-000000000000"), "PAR0053", new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 3", "Decorative", 20 },
                    { new Guid("50000000-0006-0003-0000-000000000000"), "PAR0054", new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 4", "Structural", 25 },
                    { new Guid("50000000-0006-0004-0000-000000000000"), "PAR0055", new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 5", "Mechanical", 30 },
                    { new Guid("50000000-0006-0005-0000-000000000000"), "PAR0056", new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 6", "Decorative", 35 },
                    { new Guid("50000000-0006-0006-0000-000000000000"), "PAR0057", new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 7", "Structural", 40 },
                    { new Guid("50000000-0006-0007-0000-000000000000"), "PAR0058", new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 8", "Mechanical", 45 },
                    { new Guid("50000000-0006-0008-0000-000000000000"), "PAR0059", new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 9", "Decorative", 50 },
                    { new Guid("50000000-0006-0009-0000-000000000000"), "PAR0060", new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 10", "Structural", 55 },
                    { new Guid("50000000-0007-0000-0000-000000000000"), "PAR0061", new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 1", "Structural", 10 },
                    { new Guid("50000000-0007-0001-0000-000000000000"), "PAR0062", new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 2", "Mechanical", 15 },
                    { new Guid("50000000-0007-0002-0000-000000000000"), "PAR0063", new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 3", "Decorative", 20 },
                    { new Guid("50000000-0007-0003-0000-000000000000"), "PAR0064", new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 4", "Structural", 25 },
                    { new Guid("50000000-0007-0004-0000-000000000000"), "PAR0065", new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 5", "Mechanical", 30 },
                    { new Guid("50000000-0007-0005-0000-000000000000"), "PAR0066", new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 6", "Decorative", 35 },
                    { new Guid("50000000-0007-0006-0000-000000000000"), "PAR0067", new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 7", "Structural", 40 },
                    { new Guid("50000000-0007-0007-0000-000000000000"), "PAR0068", new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 8", "Mechanical", 45 },
                    { new Guid("50000000-0007-0008-0000-000000000000"), "PAR0069", new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 9", "Decorative", 50 },
                    { new Guid("50000000-0007-0009-0000-000000000000"), "PAR0070", new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 10", "Structural", 55 },
                    { new Guid("50000000-0008-0000-0000-000000000000"), "PAR0071", new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 1", "Structural", 10 },
                    { new Guid("50000000-0008-0001-0000-000000000000"), "PAR0072", new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 2", "Mechanical", 15 },
                    { new Guid("50000000-0008-0002-0000-000000000000"), "PAR0073", new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 3", "Decorative", 20 },
                    { new Guid("50000000-0008-0003-0000-000000000000"), "PAR0074", new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 4", "Structural", 25 },
                    { new Guid("50000000-0008-0004-0000-000000000000"), "PAR0075", new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 5", "Mechanical", 30 },
                    { new Guid("50000000-0008-0005-0000-000000000000"), "PAR0076", new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 6", "Decorative", 35 },
                    { new Guid("50000000-0008-0006-0000-000000000000"), "PAR0077", new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 7", "Structural", 40 },
                    { new Guid("50000000-0008-0007-0000-000000000000"), "PAR0078", new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 8", "Mechanical", 45 },
                    { new Guid("50000000-0008-0008-0000-000000000000"), "PAR0079", new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 9", "Decorative", 50 },
                    { new Guid("50000000-0008-0009-0000-000000000000"), "PAR0080", new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 10", "Structural", 55 },
                    { new Guid("50000000-0009-0000-0000-000000000000"), "PAR0081", new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 1", "Structural", 10 },
                    { new Guid("50000000-0009-0001-0000-000000000000"), "PAR0082", new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 2", "Mechanical", 15 },
                    { new Guid("50000000-0009-0002-0000-000000000000"), "PAR0083", new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 3", "Decorative", 20 },
                    { new Guid("50000000-0009-0003-0000-000000000000"), "PAR0084", new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 4", "Structural", 25 },
                    { new Guid("50000000-0009-0004-0000-000000000000"), "PAR0085", new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 5", "Mechanical", 30 },
                    { new Guid("50000000-0009-0005-0000-000000000000"), "PAR0086", new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 6", "Decorative", 35 },
                    { new Guid("50000000-0009-0006-0000-000000000000"), "PAR0087", new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 7", "Structural", 40 },
                    { new Guid("50000000-0009-0007-0000-000000000000"), "PAR0088", new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 8", "Mechanical", 45 },
                    { new Guid("50000000-0009-0008-0000-000000000000"), "PAR0089", new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 9", "Decorative", 50 },
                    { new Guid("50000000-0009-0009-0000-000000000000"), "PAR0090", new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 10", "Structural", 55 },
                    { new Guid("50000000-0010-0000-0000-000000000000"), "PAR0091", new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 1", "Structural", 10 },
                    { new Guid("50000000-0010-0001-0000-000000000000"), "PAR0092", new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 2", "Mechanical", 15 },
                    { new Guid("50000000-0010-0002-0000-000000000000"), "PAR0093", new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 3", "Decorative", 20 },
                    { new Guid("50000000-0010-0003-0000-000000000000"), "PAR0094", new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 4", "Structural", 25 },
                    { new Guid("50000000-0010-0004-0000-000000000000"), "PAR0095", new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 5", "Mechanical", 30 },
                    { new Guid("50000000-0010-0005-0000-000000000000"), "PAR0096", new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 6", "Decorative", 35 },
                    { new Guid("50000000-0010-0006-0000-000000000000"), "PAR0097", new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 7", "Structural", 40 },
                    { new Guid("50000000-0010-0007-0000-000000000000"), "PAR0098", new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 8", "Mechanical", 45 },
                    { new Guid("50000000-0010-0008-0000-000000000000"), "PAR0099", new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 9", "Decorative", 50 },
                    { new Guid("50000000-0010-0009-0000-000000000000"), "PAR0100", new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 10", "Structural", 55 }
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
                name: "material_replicas",
                schema: "instock");

            migrationBuilder.DropTable(
                name: "parts",
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
