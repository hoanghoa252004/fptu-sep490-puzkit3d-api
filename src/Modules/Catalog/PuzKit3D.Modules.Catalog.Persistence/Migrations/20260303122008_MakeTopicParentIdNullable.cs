using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuzKit3D.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeTopicParentIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "parent_id",
                schema: "catalog",
                table: "topic",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "parent_id",
                schema: "catalog",
                table: "topic",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
