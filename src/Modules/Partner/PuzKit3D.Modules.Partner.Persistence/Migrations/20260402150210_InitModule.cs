using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuzKit3D.Modules.Partner.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "partner");

            migrationBuilder.CreateTable(
                name: "import_service_configs",
                schema: "partner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    base_shipping_fee = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    country_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    country_name = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    import_tax_percentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_import_service_configs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_replicas",
                schema: "partner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    phone_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    province = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    district = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ward = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    street_address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "partners",
                schema: "partner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    contact_email = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    contact_phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    import_service_config_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partners", x => x.id);
                    table.ForeignKey(
                        name: "fk_partners_import_service_configs_import_service_config_id",
                        column: x => x.import_service_config_id,
                        principalSchema: "partner",
                        principalTable: "import_service_configs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "partner_product_requests",
                schema: "partner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    partner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_requested_quantity = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_product_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_product_requests_partners_partner_id",
                        column: x => x.partner_id,
                        principalSchema: "partner",
                        principalTable: "partners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "partner_products",
                schema: "partner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    partner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    reference_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    thumbnail_url = table.Column<string>(type: "text", nullable: false),
                    preview_asset = table.Column<string>(type: "jsonb", nullable: false),
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_products_partners_partner_id",
                        column: x => x.partner_id,
                        principalSchema: "partner",
                        principalTable: "partners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "partner_product_quotations",
                schema: "partner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    partner_product_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sub_total_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    shipping_fee = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    import_tax_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    grand_total_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_product_quotations", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_product_quotations_partner_product_requests_partner",
                        column: x => x.partner_product_request_id,
                        principalSchema: "partner",
                        principalTable: "partner_product_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "partner_product_request_item",
                schema: "partner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    partner_product_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    partner_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reference_unit_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    reference_total_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_product_request_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_product_request_item_partner_product_requests_partn",
                        column: x => x.partner_product_request_id,
                        principalSchema: "partner",
                        principalTable: "partner_product_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_partner_product_request_item_partner_products_partner_produ",
                        column: x => x.partner_product_id,
                        principalSchema: "partner",
                        principalTable: "partner_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "partner_product_orders",
                schema: "partner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    partner_product_quotation_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    sub_total_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    shipping_fee = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    import_tax_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    used_coin_amount_as_money = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    grand_total_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    payment_method = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "ONLINE"),
                    is_paid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_product_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_product_orders_partner_product_quotations_partner_p",
                        column: x => x.partner_product_quotation_id,
                        principalSchema: "partner",
                        principalTable: "partner_product_quotations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "partner_product_quotation_details",
                schema: "partner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    partner_product_quotation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    partner_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_product_quotation_details", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_product_quotation_details_partner_product_quotation",
                        column: x => x.partner_product_quotation_id,
                        principalSchema: "partner",
                        principalTable: "partner_product_quotations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_partner_product_quotation_details_partner_products_partner_",
                        column: x => x.partner_product_id,
                        principalSchema: "partner",
                        principalTable: "partner_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "partner_product_order_details",
                schema: "partner",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    partner_product_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    partner_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    partner_product_sku = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    partner_product_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    unit_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_product_order_details", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_product_order_details_partner_product_orders_partne",
                        column: x => x.partner_product_order_id,
                        principalSchema: "partner",
                        principalTable: "partner_product_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_partner_product_order_details_partner_products_partner_prod",
                        column: x => x.partner_product_id,
                        principalSchema: "partner",
                        principalTable: "partner_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_partner_product_order_details_partner_product_id",
                schema: "partner",
                table: "partner_product_order_details",
                column: "partner_product_id");

            migrationBuilder.CreateIndex(
                name: "ix_partner_product_order_details_partner_product_order_id_part",
                schema: "partner",
                table: "partner_product_order_details",
                columns: new[] { "partner_product_order_id", "partner_product_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_partner_product_orders_code",
                schema: "partner",
                table: "partner_product_orders",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_partner_product_orders_partner_product_quotation_id",
                schema: "partner",
                table: "partner_product_orders",
                column: "partner_product_quotation_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_partner_product_quotation_details_partner_product_id",
                schema: "partner",
                table: "partner_product_quotation_details",
                column: "partner_product_id");

            migrationBuilder.CreateIndex(
                name: "ix_partner_product_quotation_details_partner_product_quotation",
                schema: "partner",
                table: "partner_product_quotation_details",
                columns: new[] { "partner_product_quotation_id", "partner_product_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_partner_product_quotations_partner_product_request_id",
                schema: "partner",
                table: "partner_product_quotations",
                column: "partner_product_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_partner_product_request_item_partner_product_id",
                schema: "partner",
                table: "partner_product_request_item",
                column: "partner_product_id");

            migrationBuilder.CreateIndex(
                name: "ix_partner_product_request_item_partner_product_request_id_par",
                schema: "partner",
                table: "partner_product_request_item",
                columns: new[] { "partner_product_request_id", "partner_product_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_partner_product_requests_code",
                schema: "partner",
                table: "partner_product_requests",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_partner_product_requests_partner_id",
                schema: "partner",
                table: "partner_product_requests",
                column: "partner_id");

            migrationBuilder.CreateIndex(
                name: "ix_partner_products_partner_id_slug",
                schema: "partner",
                table: "partner_products",
                columns: new[] { "partner_id", "slug" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_partners_import_service_config_id",
                schema: "partner",
                table: "partners",
                column: "import_service_config_id");

            migrationBuilder.CreateIndex(
                name: "ix_partners_slug",
                schema: "partner",
                table: "partners",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__user_replica__email",
                schema: "partner",
                table: "user_replicas",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__user_replica__phone_number",
                schema: "partner",
                table: "user_replicas",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "partner_product_order_details",
                schema: "partner");

            migrationBuilder.DropTable(
                name: "partner_product_quotation_details",
                schema: "partner");

            migrationBuilder.DropTable(
                name: "partner_product_request_item",
                schema: "partner");

            migrationBuilder.DropTable(
                name: "user_replicas",
                schema: "partner");

            migrationBuilder.DropTable(
                name: "partner_product_orders",
                schema: "partner");

            migrationBuilder.DropTable(
                name: "partner_products",
                schema: "partner");

            migrationBuilder.DropTable(
                name: "partner_product_quotations",
                schema: "partner");

            migrationBuilder.DropTable(
                name: "partner_product_requests",
                schema: "partner");

            migrationBuilder.DropTable(
                name: "partners",
                schema: "partner");

            migrationBuilder.DropTable(
                name: "import_service_configs",
                schema: "partner");
        }
    }
}
