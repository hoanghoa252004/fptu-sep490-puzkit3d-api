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
                    capability_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    { new Guid("0cfb2a37-0b42-4733-8ad6-274e8c5b10ca"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("88fe0026-6d8d-4651-a0e8-05fbd25719ec"), 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("36f349df-009a-4132-ba9a-05589996c654"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("30bd1070-3f2a-44cc-b91c-3a3190ccde3e"), 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("612ce005-bc49-4194-9613-634bc8034f69"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("610abe63-8470-4b3a-ad9a-1327e711d3e1"), 118, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("6495a37c-b9ef-496d-9412-539eb0119603"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("81078602-8fb8-4c99-8eef-c60020f8799e"), 28, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("661d9590-da55-4687-8725-93fdf6432feb"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("92d31ab6-d1c0-4cfd-a51f-8ded3ab4eba7"), 96, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("6cf6a053-0366-4ee9-923d-0b8cde1802bb"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("7c490499-23be-4e85-b007-d9494d0ce90b"), 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("71296fb4-654c-47a6-a612-6f6245e364d2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a23e0996-b04f-4214-983a-efd4885a5423"), 148, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("7cd49b34-0576-4dc7-a583-9793e8cf606a"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("767d1407-834e-4eac-abf0-762b7d5c7096"), 128, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("870bbe66-6f8a-477e-882d-df58a4f42982"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("493b86cd-277d-45b0-b522-8bdcbee35ed0"), 160, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("8c05b416-5015-4893-9419-8f73b913ccef"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("fb64925f-fb87-4a82-b03f-5d61509ccb1c"), 151, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("9094b066-bfe1-48e3-a569-366227bf6807"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("5723a6e2-ba61-4e7e-a971-8d460397b86c"), 126, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a449fe6e-ec91-4b80-af62-32cb2ac3eee2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("b6a3fece-ac74-49e8-b988-18e789b5207d"), 165, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a9cd558e-f709-4baa-b75a-1cf21ffd1eb8"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("459a7e14-9efd-445c-9f72-6807b2d95f91"), 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b148d337-9d45-4f91-9d81-380d3145ed8e"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("e0e7db3f-47eb-493a-85f1-e3acdfdac4ba"), 198, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d2d2e8ba-c176-4c27-944d-c3be81169017"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("0a316984-2b37-41b9-ba1a-5c9d9f085b0b"), 152, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d2e67a39-782e-49cd-9c61-51962e1334ea"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("623542c9-83d2-47b8-a778-2dee02ce4a16"), 189, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
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
                    { new Guid("048472a2-dbda-492d-952c-bbaa97468bac"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("30bd1070-3f2a-44cc-b91c-3a3190ccde3e"), true, 288000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("0e171e54-350a-41a6-95c4-02a965167cb1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("81078602-8fb8-4c99-8eef-c60020f8799e"), true, 569000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("1058d95b-fb91-48b2-b104-ca88029c95b5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("0a316984-2b37-41b9-ba1a-5c9d9f085b0b"), true, 218400.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("1429cb64-4694-49d7-bd5d-3688552d8537"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("88fe0026-6d8d-4651-a0e8-05fbd25719ec"), true, 118400.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("3777633f-d083-4b4e-b331-75fa631f6fd5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("610abe63-8470-4b3a-ad9a-1327e711d3e1"), true, 132000.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("4564bd30-fa92-453a-895c-011099fd7394"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("a23e0996-b04f-4214-983a-efd4885a5423"), true, 831000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46f1130a-fa14-4d7c-b592-869a09d40cf1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("7c490499-23be-4e85-b007-d9494d0ce90b"), true, 412000.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("52381788-6bbd-4028-a71f-72bd73c1f385"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("b6a3fece-ac74-49e8-b988-18e789b5207d"), true, 473600.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("52ee37e7-aac4-492e-b549-b45a02ba96ef"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("a23e0996-b04f-4214-983a-efd4885a5423"), true, 664800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("568fa989-aef8-4b27-8f05-1aca82eba0d2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("fb64925f-fb87-4a82-b03f-5d61509ccb1c"), true, 80800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("5a5ccbd0-6971-4cf0-8052-88afc113027c"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("81078602-8fb8-4c99-8eef-c60020f8799e"), true, 455200.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("69eabb35-d47a-4f6e-8f88-c5ab0649759a"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("610abe63-8470-4b3a-ad9a-1327e711d3e1"), true, 165000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("7842ff7e-dd96-419b-9736-421b01a8b6e7"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("fb64925f-fb87-4a82-b03f-5d61509ccb1c"), true, 101000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("7888fd63-c583-438c-9a09-483cee7c89f0"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("b6a3fece-ac74-49e8-b988-18e789b5207d"), true, 592000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("7dd46b42-2ec2-47e2-bb96-36ed74948f26"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("493b86cd-277d-45b0-b522-8bdcbee35ed0"), true, 372800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("8561bd03-42ed-4eee-bfce-ba4c91baacdb"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("459a7e14-9efd-445c-9f72-6807b2d95f91"), true, 120800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("8ab5308c-5228-4efd-9337-7bdf48b0eb0e"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("623542c9-83d2-47b8-a778-2dee02ce4a16"), true, 106000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("8d6602cb-bfb0-41de-985b-1a1ca0e4662b"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("0a316984-2b37-41b9-ba1a-5c9d9f085b0b"), true, 273000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("935cf09f-735a-46e8-823c-d857abc4ceb6"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("7c490499-23be-4e85-b007-d9494d0ce90b"), true, 515000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("9ba11246-046c-4c3e-bb66-91699f408dd8"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("5723a6e2-ba61-4e7e-a971-8d460397b86c"), true, 220000.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a77e21a2-aa15-4945-8dc2-78e72ef6abc3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("493b86cd-277d-45b0-b522-8bdcbee35ed0"), true, 466000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("af6d2eee-a67b-40ae-a5a0-0fd7c72850bf"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("e0e7db3f-47eb-493a-85f1-e3acdfdac4ba"), true, 706400.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b3acd2d8-c79c-415a-9157-6fc82b701f8c"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("623542c9-83d2-47b8-a778-2dee02ce4a16"), true, 84800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("bcc8a646-ad5e-47f7-950b-54e68fb8375f"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("767d1407-834e-4eac-abf0-762b7d5c7096"), true, 256000.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c160ae53-0fa3-443a-9c41-55bc9eafbb37"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("767d1407-834e-4eac-abf0-762b7d5c7096"), true, 320000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c43109f7-b0f9-44b6-9cac-6f078eace997"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("92d31ab6-d1c0-4cfd-a51f-8ded3ab4eba7"), true, 246000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c49cdba6-6e6b-4aa5-9102-8776b1d24d97"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("30bd1070-3f2a-44cc-b91c-3a3190ccde3e"), true, 230400.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c60ac9ef-4b05-4bc0-8c05-61a0eb345726"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999992"), new Guid("92d31ab6-d1c0-4cfd-a51f-8ded3ab4eba7"), true, 196800.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c68ad62a-3c66-40ef-ba99-de8fef28a7b0"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("e0e7db3f-47eb-493a-85f1-e3acdfdac4ba"), true, 883000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("ec82443d-b689-43ec-a0fc-a541c946132f"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("5723a6e2-ba61-4e7e-a971-8d460397b86c"), true, 275000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f63c613e-33d3-4a6b-8837-35a1f598667d"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("459a7e14-9efd-445c-9f72-6807b2d95f91"), true, 151000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f7afb665-bf39-4698-9516-c079660d9285"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999991"), new Guid("88fe0026-6d8d-4651-a0e8-05fbd25719ec"), true, 148000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "cart",
                table: "in_stock_product_replicas",
                columns: new[] { "id", "assembly_method_id", "capability_id", "code", "created_at", "description", "difficult_level", "estimated_build_time", "is_active", "material_id", "name", "preview_asset", "slug", "thumbnail_url", "topic_id", "total_piece_count", "updated_at" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Basic", 120, true, new Guid("55555555-5555-5555-5555-555555555555"), "Lion 3D Puzzle", "{\"main\":\"https://example.com/lion-preview.jpg\"}", "lion-3d-puzzle", "https://example.com/lion.jpg", new Guid("11111111-1111-1111-1111-111111111111"), 150, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Intermediate", 180, true, new Guid("55555555-5555-5555-5555-555555555555"), "Elephant 3D Puzzle", "{\"main\":\"https://example.com/elephant-preview.jpg\"}", "elephant-3d-puzzle", "https://example.com/elephant.jpg", new Guid("11111111-1111-1111-1111-111111111111"), 200, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("88888888-8888-8888-8888-888888888888"), "INP003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Advanced", 240, true, new Guid("55555555-5555-5555-5555-555555555555"), "Eagle 3D Puzzle", "{\"main\":\"https://example.com/eagle-preview.jpg\"}", "eagle-3d-puzzle", "https://example.com/eagle.jpg", new Guid("11111111-1111-1111-1111-111111111111"), 180, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("88888888-8888-8888-8888-888888888888"), "INP004", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Advanced", 300, true, new Guid("66666666-6666-6666-6666-666666666666"), "Sports Car 3D Puzzle", "{\"main\":\"https://example.com/sports-car-preview.jpg\"}", "sports-car-3d-puzzle", "https://example.com/sports-car.jpg", new Guid("22222222-2222-2222-2222-222222222222"), 250, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP005", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Intermediate", 200, true, new Guid("66666666-6666-6666-6666-666666666666"), "Airplane 3D Puzzle", "{\"main\":\"https://example.com/airplane-preview.jpg\"}", "airplane-3d-puzzle", "https://example.com/airplane.jpg", new Guid("22222222-2222-2222-2222-222222222222"), 220, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP006", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Intermediate", 150, true, new Guid("66666666-6666-6666-6666-666666666666"), "Motorcycle 3D Puzzle", "{\"main\":\"https://example.com/motorcycle-preview.jpg\"}", "motorcycle-3d-puzzle", "https://example.com/motorcycle.jpg", new Guid("22222222-2222-2222-2222-222222222222"), 180, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP007", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Basic", 130, true, new Guid("55555555-5555-5555-5555-555555555555"), "Tiger 3D Puzzle", "{\"main\":\"https://example.com/tiger-preview.jpg\"}", "tiger-3d-puzzle", "https://example.com/tiger.jpg", new Guid("11111111-1111-1111-1111-111111111111"), 170, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP008", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Basic", 100, true, new Guid("55555555-5555-5555-5555-555555555555"), "Dolphin 3D Puzzle", "{\"main\":\"https://example.com/dolphin-preview.jpg\"}", "dolphin-3d-puzzle", "https://example.com/dolphin.jpg", new Guid("11111111-1111-1111-1111-111111111111"), 130, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), "INP009", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Intermediate", 170, true, new Guid("66666666-6666-6666-6666-666666666666"), "Helicopter 3D Puzzle", "{\"main\":\"https://example.com/helicopter-preview.jpg\"}", "helicopter-3d-puzzle", "https://example.com/helicopter.jpg", new Guid("22222222-2222-2222-2222-222222222222"), 190, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000010"), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("88888888-8888-8888-8888-888888888888"), "INP010", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Advanced", 360, true, new Guid("55555555-5555-5555-5555-555555555555"), "Dragon 3D Puzzle", "{\"main\":\"https://example.com/dragon-preview.jpg\"}", "dragon-3d-puzzle", "https://example.com/dragon.jpg", new Guid("11111111-1111-1111-1111-111111111111"), 300, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "cart",
                table: "in_stock_product_variant_replicas",
                columns: new[] { "id", "assembled_height_mm", "assembled_length_mm", "assembled_width_mm", "color", "created_at", "in_stock_product_id", "is_active", "sku", "updated_at" },
                values: new object[,]
                {
                    { new Guid("0a316984-2b37-41b9-ba1a-5c9d9f085b0b"), 198, 286, 277, "Red", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, "SKU009", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30bd1070-3f2a-44cc-b91c-3a3190ccde3e"), 269, 285, 172, "Orange", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, "SKU015", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("459a7e14-9efd-445c-9f72-6807b2d95f91"), 248, 292, 125, "Yellow", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, "SKU012", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("493b86cd-277d-45b0-b522-8bdcbee35ed0"), 141, 118, 283, "Purple", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, "SKU008", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("5723a6e2-ba61-4e7e-a971-8d460397b86c"), 129, 109, 103, "Blue", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, "SKU002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("610abe63-8470-4b3a-ad9a-1327e711d3e1"), 251, 219, 270, "Orange", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, "SKU007", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("623542c9-83d2-47b8-a778-2dee02ce4a16"), 151, 269, 213, "Yellow", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, "SKU004", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("767d1407-834e-4eac-abf0-762b7d5c7096"), 217, 154, 206, "Purple", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, "SKU016", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("7c490499-23be-4e85-b007-d9494d0ce90b"), 137, 281, 199, "Green", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, "SKU003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("81078602-8fb8-4c99-8eef-c60020f8799e"), 279, 227, 237, "White", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, "SKU006", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("88fe0026-6d8d-4651-a0e8-05fbd25719ec"), 225, 172, 123, "Black", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, "SKU013", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("92d31ab6-d1c0-4cfd-a51f-8ded3ab4eba7"), 176, 122, 136, "Blue", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, "SKU010", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a23e0996-b04f-4214-983a-efd4885a5423"), 248, 296, 281, "Red", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, "SKU001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b6a3fece-ac74-49e8-b988-18e789b5207d"), 102, 240, 130, "Green", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), true, "SKU011", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e0e7db3f-47eb-493a-85f1-e3acdfdac4ba"), 123, 111, 247, "White", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), true, "SKU014", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("fb64925f-fb87-4a82-b03f-5d61509ccb1c"), 255, 131, 104, "Black", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, "SKU005", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
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
