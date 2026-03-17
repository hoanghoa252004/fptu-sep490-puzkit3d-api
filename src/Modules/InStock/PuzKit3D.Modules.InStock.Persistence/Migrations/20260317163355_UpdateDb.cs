using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuzKit3D.Modules.InStock.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "delivery_order_code",
                schema: "instock",
                table: "instock_orders",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "expected_delivery_date",
                schema: "instock",
                table: "instock_orders",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "delivery_order_code",
                schema: "instock",
                table: "instock_orders");

            migrationBuilder.DropColumn(
                name: "expected_delivery_date",
                schema: "instock",
                table: "instock_orders");
        }
    }
}
