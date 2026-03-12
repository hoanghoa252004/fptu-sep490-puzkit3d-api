using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuzKit3D.Modules.Cart.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnitPriceFromCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unit_price_amount",
                schema: "cart",
                table: "cart_items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "unit_price_amount",
                schema: "cart",
                table: "cart_items",
                type: "numeric(10,2)",
                nullable: true);
        }
    }
}
