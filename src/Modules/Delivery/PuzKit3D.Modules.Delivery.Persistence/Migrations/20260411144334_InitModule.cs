using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PuzKit3D.Modules.Delivery.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "delivery");

            migrationBuilder.CreateTable(
                name: "delivery_trackings",
                schema: "delivery",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    support_ticket_id = table.Column<Guid>(type: "uuid", nullable: true),
                    delivery_order_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    hand_over_image_url = table.Column<string>(type: "text", nullable: true),
                    expected_delivery_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    delivered_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_delivery_trackings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "drive_replicas",
                schema: "delivery",
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
                name: "order_detail_replicas",
                schema: "delivery",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    product_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    variant_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_detail_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_replicas",
                schema: "delivery",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "part_replicas",
                schema: "delivery",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    part_type = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    instock_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_part_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "support_ticket_replicas",
                schema: "delivery",
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
                name: "user_replicas",
                schema: "delivery",
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
                name: "delivery_tracking_details",
                schema: "delivery",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    delivery_tracking_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_delivery_tracking_details", x => x.id);
                    table.ForeignKey(
                        name: "fk_delivery_tracking_details_delivery_trackings_delivery_track",
                        column: x => x.delivery_tracking_id,
                        principalSchema: "delivery",
                        principalTable: "delivery_trackings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "support_ticket_detail_replicas",
                schema: "delivery",
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
                        principalSchema: "delivery",
                        principalTable: "support_ticket_replicas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "delivery",
                table: "part_replicas",
                columns: new[] { "id", "code", "created_at", "instock_product_id", "name", "part_type", "quantity", "updated_at" },
                values: new object[,]
                {
                    { new Guid("50000000-0001-0000-0000-000000000000"), "PAR0001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 1", "Structural", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0001-0001-0000-000000000000"), "PAR0002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 2", "Mechanical", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0001-0002-0000-000000000000"), "PAR0003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 3", "Decorative", 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0001-0003-0000-000000000000"), "PAR0004", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 4", "Structural", 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0001-0004-0000-000000000000"), "PAR0005", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 5", "Mechanical", 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0001-0005-0000-000000000000"), "PAR0006", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 6", "Decorative", 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0001-0006-0000-000000000000"), "PAR0007", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 7", "Structural", 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0001-0007-0000-000000000000"), "PAR0008", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 8", "Mechanical", 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0001-0008-0000-000000000000"), "PAR0009", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 9", "Decorative", 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0001-0009-0000-000000000000"), "PAR0010", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), "Lion Part 10", "Structural", 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0002-0000-0000-000000000000"), "PAR0011", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 1", "Structural", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0002-0001-0000-000000000000"), "PAR0012", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 2", "Mechanical", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0002-0002-0000-000000000000"), "PAR0013", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 3", "Decorative", 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0002-0003-0000-000000000000"), "PAR0014", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 4", "Structural", 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0002-0004-0000-000000000000"), "PAR0015", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 5", "Mechanical", 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0002-0005-0000-000000000000"), "PAR0016", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 6", "Decorative", 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0002-0006-0000-000000000000"), "PAR0017", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 7", "Structural", 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0002-0007-0000-000000000000"), "PAR0018", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 8", "Mechanical", 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0002-0008-0000-000000000000"), "PAR0019", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 9", "Decorative", 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0002-0009-0000-000000000000"), "PAR0020", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), "Elephant Part 10", "Structural", 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0003-0000-0000-000000000000"), "PAR0021", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 1", "Structural", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0003-0001-0000-000000000000"), "PAR0022", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 2", "Mechanical", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0003-0002-0000-000000000000"), "PAR0023", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 3", "Decorative", 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0003-0003-0000-000000000000"), "PAR0024", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 4", "Structural", 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0003-0004-0000-000000000000"), "PAR0025", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 5", "Mechanical", 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0003-0005-0000-000000000000"), "PAR0026", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 6", "Decorative", 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0003-0006-0000-000000000000"), "PAR0027", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 7", "Structural", 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0003-0007-0000-000000000000"), "PAR0028", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 8", "Mechanical", 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0003-0008-0000-000000000000"), "PAR0029", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 9", "Decorative", 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0003-0009-0000-000000000000"), "PAR0030", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), "Eagle Part 10", "Structural", 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0004-0000-0000-000000000000"), "PAR0031", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 1", "Structural", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0004-0001-0000-000000000000"), "PAR0032", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 2", "Mechanical", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0004-0002-0000-000000000000"), "PAR0033", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 3", "Decorative", 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0004-0003-0000-000000000000"), "PAR0034", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 4", "Structural", 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0004-0004-0000-000000000000"), "PAR0035", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 5", "Mechanical", 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0004-0005-0000-000000000000"), "PAR0036", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 6", "Decorative", 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0004-0006-0000-000000000000"), "PAR0037", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 7", "Structural", 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0004-0007-0000-000000000000"), "PAR0038", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 8", "Mechanical", 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0004-0008-0000-000000000000"), "PAR0039", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 9", "Decorative", 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0004-0009-0000-000000000000"), "PAR0040", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car Part 10", "Structural", 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0005-0000-0000-000000000000"), "PAR0041", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 1", "Structural", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0005-0001-0000-000000000000"), "PAR0042", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 2", "Mechanical", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0005-0002-0000-000000000000"), "PAR0043", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 3", "Decorative", 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0005-0003-0000-000000000000"), "PAR0044", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 4", "Structural", 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0005-0004-0000-000000000000"), "PAR0045", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 5", "Mechanical", 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0005-0005-0000-000000000000"), "PAR0046", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 6", "Decorative", 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0005-0006-0000-000000000000"), "PAR0047", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 7", "Structural", 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0005-0007-0000-000000000000"), "PAR0048", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 8", "Mechanical", 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0005-0008-0000-000000000000"), "PAR0049", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 9", "Decorative", 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0005-0009-0000-000000000000"), "PAR0050", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), "Airplane Part 10", "Structural", 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0006-0000-0000-000000000000"), "PAR0051", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 1", "Structural", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0006-0001-0000-000000000000"), "PAR0052", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 2", "Mechanical", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0006-0002-0000-000000000000"), "PAR0053", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 3", "Decorative", 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0006-0003-0000-000000000000"), "PAR0054", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 4", "Structural", 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0006-0004-0000-000000000000"), "PAR0055", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 5", "Mechanical", 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0006-0005-0000-000000000000"), "PAR0056", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 6", "Decorative", 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0006-0006-0000-000000000000"), "PAR0057", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 7", "Structural", 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0006-0007-0000-000000000000"), "PAR0058", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 8", "Mechanical", 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0006-0008-0000-000000000000"), "PAR0059", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 9", "Decorative", 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0006-0009-0000-000000000000"), "PAR0060", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle Part 10", "Structural", 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0007-0000-0000-000000000000"), "PAR0061", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 1", "Structural", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0007-0001-0000-000000000000"), "PAR0062", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 2", "Mechanical", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0007-0002-0000-000000000000"), "PAR0063", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 3", "Decorative", 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0007-0003-0000-000000000000"), "PAR0064", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 4", "Structural", 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0007-0004-0000-000000000000"), "PAR0065", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 5", "Mechanical", 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0007-0005-0000-000000000000"), "PAR0066", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 6", "Decorative", 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0007-0006-0000-000000000000"), "PAR0067", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 7", "Structural", 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0007-0007-0000-000000000000"), "PAR0068", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 8", "Mechanical", 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0007-0008-0000-000000000000"), "PAR0069", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 9", "Decorative", 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0007-0009-0000-000000000000"), "PAR0070", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), "Tiger Part 10", "Structural", 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0008-0000-0000-000000000000"), "PAR0071", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 1", "Structural", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0008-0001-0000-000000000000"), "PAR0072", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 2", "Mechanical", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0008-0002-0000-000000000000"), "PAR0073", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 3", "Decorative", 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0008-0003-0000-000000000000"), "PAR0074", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 4", "Structural", 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0008-0004-0000-000000000000"), "PAR0075", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 5", "Mechanical", 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0008-0005-0000-000000000000"), "PAR0076", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 6", "Decorative", 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0008-0006-0000-000000000000"), "PAR0077", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 7", "Structural", 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0008-0007-0000-000000000000"), "PAR0078", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 8", "Mechanical", 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0008-0008-0000-000000000000"), "PAR0079", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 9", "Decorative", 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0008-0009-0000-000000000000"), "PAR0080", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin Part 10", "Structural", 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0009-0000-0000-000000000000"), "PAR0081", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 1", "Structural", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0009-0001-0000-000000000000"), "PAR0082", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 2", "Mechanical", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0009-0002-0000-000000000000"), "PAR0083", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 3", "Decorative", 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0009-0003-0000-000000000000"), "PAR0084", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 4", "Structural", 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0009-0004-0000-000000000000"), "PAR0085", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 5", "Mechanical", 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0009-0005-0000-000000000000"), "PAR0086", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 6", "Decorative", 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0009-0006-0000-000000000000"), "PAR0087", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 7", "Structural", 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0009-0007-0000-000000000000"), "PAR0088", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 8", "Mechanical", 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0009-0008-0000-000000000000"), "PAR0089", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 9", "Decorative", 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0009-0009-0000-000000000000"), "PAR0090", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter Part 10", "Structural", 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0010-0000-0000-000000000000"), "PAR0091", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 1", "Structural", 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0010-0001-0000-000000000000"), "PAR0092", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 2", "Mechanical", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0010-0002-0000-000000000000"), "PAR0093", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 3", "Decorative", 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0010-0003-0000-000000000000"), "PAR0094", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 4", "Structural", 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0010-0004-0000-000000000000"), "PAR0095", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 5", "Mechanical", 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0010-0005-0000-000000000000"), "PAR0096", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 6", "Decorative", 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0010-0006-0000-000000000000"), "PAR0097", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 7", "Structural", 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0010-0007-0000-000000000000"), "PAR0098", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 8", "Mechanical", 45, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0010-0008-0000-000000000000"), "PAR0099", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 9", "Decorative", 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0010-0009-0000-000000000000"), "PAR0100", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), "Dragon Part 10", "Structural", 55, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "ix_delivery_tracking_details_delivery_tracking_id",
                schema: "delivery",
                table: "delivery_tracking_details",
                column: "delivery_tracking_id");

            migrationBuilder.CreateIndex(
                name: "ix_delivery_tracking_details_item_id",
                schema: "delivery",
                table: "delivery_tracking_details",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "ix_delivery_trackings_created_at",
                schema: "delivery",
                table: "delivery_trackings",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_delivery_trackings_delivery_order_code",
                schema: "delivery",
                table: "delivery_trackings",
                column: "delivery_order_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_delivery_trackings_order_id",
                schema: "delivery",
                table: "delivery_trackings",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_delivery_trackings_status",
                schema: "delivery",
                table: "delivery_trackings",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_delivery_trackings_support_ticket_id",
                schema: "delivery",
                table: "delivery_trackings",
                column: "support_ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_delivery_trackings_type",
                schema: "delivery",
                table: "delivery_trackings",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "ix_order_detail_replicas_order_id",
                schema: "delivery",
                table: "order_detail_replicas",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_detail_replicas_product_id",
                schema: "delivery",
                table: "order_detail_replicas",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_replicas_customer_id",
                schema: "delivery",
                table: "order_replicas",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_ticket_detail_replicas_order_item_id",
                schema: "delivery",
                table: "support_ticket_detail_replicas",
                column: "order_item_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_ticket_detail_replicas_support_ticket_id",
                schema: "delivery",
                table: "support_ticket_detail_replicas",
                column: "support_ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_ticket_replicas_order_id",
                schema: "delivery",
                table: "support_ticket_replicas",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_ticket_replicas_status",
                schema: "delivery",
                table: "support_ticket_replicas",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_support_ticket_replicas_user_id",
                schema: "delivery",
                table: "support_ticket_replicas",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UK__user_replica__email",
                schema: "delivery",
                table: "user_replicas",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__user_replica__phone_number",
                schema: "delivery",
                table: "user_replicas",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "delivery_tracking_details",
                schema: "delivery");

            migrationBuilder.DropTable(
                name: "drive_replicas",
                schema: "delivery");

            migrationBuilder.DropTable(
                name: "order_detail_replicas",
                schema: "delivery");

            migrationBuilder.DropTable(
                name: "order_replicas",
                schema: "delivery");

            migrationBuilder.DropTable(
                name: "part_replicas",
                schema: "delivery");

            migrationBuilder.DropTable(
                name: "support_ticket_detail_replicas",
                schema: "delivery");

            migrationBuilder.DropTable(
                name: "user_replicas",
                schema: "delivery");

            migrationBuilder.DropTable(
                name: "delivery_trackings",
                schema: "delivery");

            migrationBuilder.DropTable(
                name: "support_ticket_replicas",
                schema: "delivery");
        }
    }
}
