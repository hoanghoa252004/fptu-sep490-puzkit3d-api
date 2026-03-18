using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuzKit3D.Modules.InStock.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInstockOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "customer_district_code",
                schema: "instock",
                table: "instock_orders");

            migrationBuilder.DropColumn(
                name: "customer_province_code",
                schema: "instock",
                table: "instock_orders");

            migrationBuilder.DropColumn(
                name: "customer_ward_code",
                schema: "instock",
                table: "instock_orders");

            migrationBuilder.AddColumn<string>(
                name: "detail_address",
                schema: "instock",
                table: "instock_orders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "handover_proof_image_url",
                schema: "instock",
                table: "instock_orders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "detail_address",
                schema: "instock",
                table: "instock_orders");

            migrationBuilder.DropColumn(
                name: "handover_proof_image_url",
                schema: "instock",
                table: "instock_orders");

            migrationBuilder.AddColumn<string>(
                name: "customer_district_code",
                schema: "instock",
                table: "instock_orders",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "customer_province_code",
                schema: "instock",
                table: "instock_orders",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "customer_ward_code",
                schema: "instock",
                table: "instock_orders",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
