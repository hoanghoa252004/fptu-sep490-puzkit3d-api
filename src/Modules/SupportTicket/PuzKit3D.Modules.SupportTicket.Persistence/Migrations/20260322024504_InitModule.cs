using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuzKit3D.Modules.SupportTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "completed_order_replicas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_completed_order_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "support_tickets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("pk_support_tickets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "completed_order_item_replicas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    completed_order_replica_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_completed_order_item_replicas", x => x.id);
                    table.ForeignKey(
                        name: "fk_completed_order_item_replicas_completed_order_replicas_comp",
                        column: x => x.completed_order_replica_id,
                        principalTable: "completed_order_replicas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "support_ticket_details",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    support_ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    part_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_support_ticket_details", x => x.id);
                    table.ForeignKey(
                        name: "fk_support_ticket_details_support_tickets_support_ticket_id",
                        column: x => x.support_ticket_id,
                        principalTable: "support_tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_completed_order_item_replicas_completed_order_replica_id",
                table: "completed_order_item_replicas",
                column: "completed_order_replica_id");

            migrationBuilder.CreateIndex(
                name: "ix_completed_order_item_replicas_product_id",
                table: "completed_order_item_replicas",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_completed_order_replicas_customer_id",
                table: "completed_order_replicas",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_ticket_details_part_id",
                table: "support_ticket_details",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_ticket_details_support_ticket_id",
                table: "support_ticket_details",
                column: "support_ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_tickets_order_id",
                table: "support_tickets",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_tickets_status",
                table: "support_tickets",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_support_tickets_user_id",
                table: "support_tickets",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "completed_order_item_replicas");

            migrationBuilder.DropTable(
                name: "support_ticket_details");

            migrationBuilder.DropTable(
                name: "completed_order_replicas");

            migrationBuilder.DropTable(
                name: "support_tickets");
        }
    }
}
