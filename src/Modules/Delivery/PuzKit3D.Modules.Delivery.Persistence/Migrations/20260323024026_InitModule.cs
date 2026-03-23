using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                name: "order_detail_replicas",
                schema: "delivery",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "delivery_tracking_details",
                schema: "delivery");

            migrationBuilder.DropTable(
                name: "order_detail_replicas",
                schema: "delivery");

            migrationBuilder.DropTable(
                name: "order_replicas",
                schema: "delivery");

            migrationBuilder.DropTable(
                name: "delivery_trackings",
                schema: "delivery");
        }
    }
}
