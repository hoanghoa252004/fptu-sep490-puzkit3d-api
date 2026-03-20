using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuzKit3D.Modules.Feedback.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "feedback");

            migrationBuilder.CreateTable(
                name: "completed_order_replicas",
                schema: "feedback",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_completed_order_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "feedbacks",
                schema: "feedback",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feedbacks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_replicas",
                schema: "feedback",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_replicas", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_completed_order_replicas_code",
                schema: "feedback",
                table: "completed_order_replicas",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_completed_order_replicas_customer_id",
                schema: "feedback",
                table: "completed_order_replicas",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_completed_order_replicas_product_id",
                schema: "feedback",
                table: "completed_order_replicas",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_feedbacks_order_id_user_id",
                schema: "feedback",
                table: "feedbacks",
                columns: new[] { "order_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_feedbacks_user_id",
                schema: "feedback",
                table: "feedbacks",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_replicas_type",
                schema: "feedback",
                table: "product_replicas",
                column: "type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "completed_order_replicas",
                schema: "feedback");

            migrationBuilder.DropTable(
                name: "feedbacks",
                schema: "feedback");

            migrationBuilder.DropTable(
                name: "product_replicas",
                schema: "feedback");
        }
    }
}
