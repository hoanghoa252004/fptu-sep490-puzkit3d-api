using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuzKit3D.Modules.Payment.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "payment");

            migrationBuilder.CreateTable(
                name: "order_replicas",
                schema: "payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    payment_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_paid = table.Column<bool>(type: "boolean", nullable: false),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                schema: "payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reference_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reference_order_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    expired_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                schema: "payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    txn_ref = table.Column<string>(type: "text", nullable: false),
                    payment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    transaction_no = table.Column<string>(type: "text", nullable: true),
                    payment_url = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    raw_response_payload = table.Column<string>(type: "text", nullable: true),
                    expired_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_transactions_payments_payment_id",
                        column: x => x.payment_id,
                        principalSchema: "payment",
                        principalTable: "payments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_replicas_code",
                schema: "payment",
                table: "order_replicas",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "ix_order_replicas_customer_id",
                schema: "payment",
                table: "order_replicas",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_replicas_type",
                schema: "payment",
                table: "order_replicas",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "ix_payments_reference_order_id",
                schema: "payment",
                table: "payments",
                column: "reference_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_status",
                schema: "payment",
                table: "payments",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_payment_id",
                schema: "payment",
                table: "transactions",
                column: "payment_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_status",
                schema: "payment",
                table: "transactions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_txn_ref",
                schema: "payment",
                table: "transactions",
                column: "txn_ref",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_replicas",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "transactions",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "payments",
                schema: "payment");
        }
    }
}
