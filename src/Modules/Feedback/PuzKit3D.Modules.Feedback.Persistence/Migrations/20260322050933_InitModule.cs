using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                name: "order_detail_replicas",
                schema: "feedback",
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
                schema: "feedback",
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

            migrationBuilder.InsertData(
                schema: "feedback",
                table: "product_replicas",
                columns: new[] { "id", "name", "type" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "UGT-24 Endurance Racer", "Instock" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "Mad Hornet Airplane", "Instock" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "Eagle 3D Puzzle", "Instock" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "Sports Car 3D Puzzle", "Instock" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Airplane 3D Puzzle", "Instock" },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "Motorcycle 3D Puzzle", "Instock" },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "Tiger 3D Puzzle", "Instock" },
                    { new Guid("10000000-0000-0000-0000-000000000008"), "Dolphin 3D Puzzle", "Instock" },
                    { new Guid("10000000-0000-0000-0000-000000000009"), "Helicopter 3D Puzzle", "Instock" },
                    { new Guid("10000000-0000-0000-0000-000000000010"), "Dragon 3D Puzzle", "Instock" }
                });

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
                name: "ix_order_detail_replicas_order_id",
                schema: "feedback",
                table: "order_detail_replicas",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_detail_replicas_product_id",
                schema: "feedback",
                table: "order_detail_replicas",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_replicas_customer_id",
                schema: "feedback",
                table: "order_replicas",
                column: "customer_id");

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
                name: "feedbacks",
                schema: "feedback");

            migrationBuilder.DropTable(
                name: "order_detail_replicas",
                schema: "feedback");

            migrationBuilder.DropTable(
                name: "order_replicas",
                schema: "feedback");

            migrationBuilder.DropTable(
                name: "product_replicas",
                schema: "feedback");
        }
    }
}
