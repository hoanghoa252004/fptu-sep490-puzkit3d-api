using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PuzKit3D.Modules.Cart.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInstockPriceReplica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__cart__cart_type",
                schema: "cart",
                table: "cart");

            migrationBuilder.DropTable(
                name: "cart_type",
                schema: "cart");

            migrationBuilder.DropIndex(
                name: "CUK___cart___user_id__cart_type_id",
                schema: "cart",
                table: "cart");

            migrationBuilder.DropIndex(
                name: "ix_cart_cart_type_id",
                schema: "cart",
                table: "cart");

            migrationBuilder.DropColumn(
                name: "cart_type_id",
                schema: "cart",
                table: "cart");

            migrationBuilder.AddColumn<string>(
                name: "cart_type",
                schema: "cart",
                table: "cart",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "instock_price_replica",
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
                    table.PrimaryKey("pk_instock_price_replica", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "CUK___cart___user_id__cart_type",
                schema: "cart",
                table: "cart",
                columns: new[] { "user_id", "cart_type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "instock_price_replica",
                schema: "cart");

            migrationBuilder.DropIndex(
                name: "CUK___cart___user_id__cart_type",
                schema: "cart",
                table: "cart");

            migrationBuilder.DropColumn(
                name: "cart_type",
                schema: "cart",
                table: "cart");

            migrationBuilder.AddColumn<Guid>(
                name: "cart_type_id",
                schema: "cart",
                table: "cart",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "cart_type",
                schema: "cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cart_type", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "cart",
                table: "cart_type",
                columns: new[] { "id", "created_at", "is_active", "name", "updated_at" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "INSTOCK", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "PARTNER", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "CUK___cart___user_id__cart_type_id",
                schema: "cart",
                table: "cart",
                columns: new[] { "user_id", "cart_type_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_cart_cart_type_id",
                schema: "cart",
                table: "cart",
                column: "cart_type_id");

            migrationBuilder.CreateIndex(
                name: "UK___cart_type_name",
                schema: "cart",
                table: "cart_type",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__cart__cart_type",
                schema: "cart",
                table: "cart",
                column: "cart_type_id",
                principalSchema: "cart",
                principalTable: "cart_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
