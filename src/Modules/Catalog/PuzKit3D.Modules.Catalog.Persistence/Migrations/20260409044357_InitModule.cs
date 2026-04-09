using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuzKit3D.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.CreateTable(
                name: "assembly_methods",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    factor_percentage = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assembly_methods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "capabilities",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    factor_percentage = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_capabilities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "drives",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    min_volume = table.Column<int>(type: "integer", nullable: true),
                    quantity_in_stock = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drives", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "formulas",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    expression = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_formulas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "materials",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    factor_percentage = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    base_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_materials", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "topics",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    factor_percentage = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_topics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "capability_drives",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    capability_id = table.Column<Guid>(type: "uuid", nullable: false),
                    drive_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_capability_drives", x => x.id);
                    table.ForeignKey(
                        name: "fk_capability_drives_capabilities_capability_id",
                        column: x => x.capability_id,
                        principalSchema: "catalog",
                        principalTable: "capabilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_capability_drives_drives_drive_id",
                        column: x => x.drive_id,
                        principalSchema: "catalog",
                        principalTable: "drives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "formula_value_validations",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    formula_id = table.Column<Guid>(type: "uuid", nullable: false),
                    min_value = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    max_value = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    output = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_formula_value_validations", x => x.id);
                    table.ForeignKey(
                        name: "fk_formula_value_validations_formulas_formula_id",
                        column: x => x.formula_id,
                        principalSchema: "catalog",
                        principalTable: "formulas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "capability_material_assemblies",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    capability_id = table.Column<Guid>(type: "uuid", nullable: false),
                    material_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assembly_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    assembly_method_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_capability_material_assemblies", x => x.id);
                    table.ForeignKey(
                        name: "fk_capability_material_assemblies_assembly_methods_assembly_me",
                        column: x => x.assembly_method_id,
                        principalSchema: "catalog",
                        principalTable: "assembly_methods",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_capability_material_assemblies_capabilities_capability_id",
                        column: x => x.capability_id,
                        principalSchema: "catalog",
                        principalTable: "capabilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_capability_material_assemblies_materials_material_id",
                        column: x => x.material_id,
                        principalSchema: "catalog",
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topic_material_capabilities",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    topic_id = table.Column<Guid>(type: "uuid", nullable: false),
                    material_id = table.Column<Guid>(type: "uuid", nullable: false),
                    capability_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_topic_material_capabilities", x => x.id);
                    table.ForeignKey(
                        name: "fk_topic_material_capabilities_capabilities_capability_id",
                        column: x => x.capability_id,
                        principalSchema: "catalog",
                        principalTable: "capabilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_topic_material_capabilities_materials_material_id",
                        column: x => x.material_id,
                        principalSchema: "catalog",
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_topic_material_capabilities_topics_topic_id",
                        column: x => x.topic_id,
                        principalSchema: "catalog",
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_assembly_methods_slug",
                schema: "catalog",
                table: "assembly_methods",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_capabilities_slug",
                schema: "catalog",
                table: "capabilities",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_capability_drives_capability_id",
                schema: "catalog",
                table: "capability_drives",
                column: "capability_id");

            migrationBuilder.CreateIndex(
                name: "ix_capability_drives_drive_id",
                schema: "catalog",
                table: "capability_drives",
                column: "drive_id");

            migrationBuilder.CreateIndex(
                name: "ix_capability_material_assemblies_assembly_method_id",
                schema: "catalog",
                table: "capability_material_assemblies",
                column: "assembly_method_id");

            migrationBuilder.CreateIndex(
                name: "ix_capability_material_assemblies_capability_id",
                schema: "catalog",
                table: "capability_material_assemblies",
                column: "capability_id");

            migrationBuilder.CreateIndex(
                name: "ix_capability_material_assemblies_material_id",
                schema: "catalog",
                table: "capability_material_assemblies",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "ix_formula_value_validations_formula_id",
                schema: "catalog",
                table: "formula_value_validations",
                column: "formula_id");

            migrationBuilder.CreateIndex(
                name: "ix_materials_slug",
                schema: "catalog",
                table: "materials",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_topic_material_capabilities_capability_id",
                schema: "catalog",
                table: "topic_material_capabilities",
                column: "capability_id");

            migrationBuilder.CreateIndex(
                name: "ix_topic_material_capabilities_material_id",
                schema: "catalog",
                table: "topic_material_capabilities",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "ix_topic_material_capabilities_topic_id",
                schema: "catalog",
                table: "topic_material_capabilities",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "ix_topics_parent_id",
                schema: "catalog",
                table: "topics",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_topics_slug",
                schema: "catalog",
                table: "topics",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "capability_drives",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "capability_material_assemblies",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "formula_value_validations",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "topic_material_capabilities",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "drives",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "assembly_methods",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "formulas",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "capabilities",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "materials",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "topics",
                schema: "catalog");
        }
    }
}
