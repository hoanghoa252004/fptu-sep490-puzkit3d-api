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
                name: "OrderReplicas",
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
                name: "payment",
                schema: "payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reference_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reference_order_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    provider = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    expired_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                schema: "payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    payment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    transaction_no = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    raw_response_payload = table.Column<string>(type: "jsonb", nullable: true),
                    expired_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transaction", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_payment_payment_id",
                        column: x => x.payment_id,
                        principalSchema: "payment",
                        principalTable: "payment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_replicas_code",
                schema: "payment",
                table: "OrderReplicas",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "ix_order_replicas_customer_id",
                schema: "payment",
                table: "OrderReplicas",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_replicas_type",
                schema: "payment",
                table: "OrderReplicas",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "ix_payment_reference_order_id",
                schema: "payment",
                table: "payment",
                column: "reference_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_payment_status",
                schema: "payment",
                table: "payment",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_code",
                schema: "payment",
                table: "transaction",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_transaction_payment_id",
                schema: "payment",
                table: "transaction",
                column: "payment_id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_status",
                schema: "payment",
                table: "transaction",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderReplicas",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "transaction",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "payment",
                schema: "payment");
        }
    }
}
