using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PuzKit3D.Modules.CustomDesign.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "custome_design");

            migrationBuilder.CreateTable(
                name: "assembly_method_replicas",
                schema: "custome_design",
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
                    table.PrimaryKey("pk_assembly_method_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "capability_replicas",
                schema: "custome_design",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    factor_percentage = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_capability_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "custom_design_requirements",
                schema: "custome_design",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    topic_id = table.Column<Guid>(type: "uuid", nullable: false),
                    material_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assembly_method_id = table.Column<Guid>(type: "uuid", nullable: false),
                    difficulty = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    min_part_quantity = table.Column<int>(type: "integer", nullable: false),
                    max_part_quantity = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_custom_design_requirements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "drive_replicas",
                schema: "custome_design",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    min_volume = table.Column<int>(type: "integer", nullable: true),
                    quantity_in_stock = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drive_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "material_replicas",
                schema: "custome_design",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    factor_percentage = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    base_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_material_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "topic_replicas",
                schema: "custome_design",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    factor_percentage = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_topic_replicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "custom_design_requests",
                schema: "custome_design",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    custom_design_requirement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    desired_length_mm = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    desired_width_mm = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    desired_height_mm = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    sketches = table.Column<string>(type: "text", nullable: true),
                    customer_prompt = table.Column<string>(type: "text", nullable: true),
                    desired_delivery_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    desired_quantity = table.Column<int>(type: "integer", nullable: false),
                    target_budget = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    used_support_concept_design_time = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_custom_design_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_custom_design_requests_custom_design_requirements_custom_de",
                        column: x => x.custom_design_requirement_id,
                        principalSchema: "custome_design",
                        principalTable: "custom_design_requirements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "requirement_capability_details",
                schema: "custome_design",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    custom_design_requirement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    capability_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requirement_capability_details", x => x.id);
                    table.ForeignKey(
                        name: "fk_requirement_capability_details_custom_design_requirements_c",
                        column: x => x.custom_design_requirement_id,
                        principalSchema: "custome_design",
                        principalTable: "custom_design_requirements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "custom_design_assets",
                schema: "custome_design",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    custom_design_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    multiview_images = table.Column<string>(type: "text", nullable: true),
                    composite_multiview_image = table.Column<string>(type: "text", nullable: true),
                    rough3d_model = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    rough3d_model_task_id = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    customer_prompt = table.Column<string>(type: "text", nullable: true),
                    normalize_prompt = table.Column<string>(type: "text", nullable: true),
                    is_need_support = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_final_design = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_custom_design_assets", x => x.id);
                    table.ForeignKey(
                        name: "fk_custom_design_assets_custom_design_requests_custom_design_r",
                        column: x => x.custom_design_request_id,
                        principalSchema: "custome_design",
                        principalTable: "custom_design_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "custome_design",
                table: "custom_design_requirements",
                columns: new[] { "id", "assembly_method_id", "code", "created_at", "difficulty", "is_active", "material_id", "max_part_quantity", "min_part_quantity", "topic_id", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a5a5a5a5-a5a5-a5a5-a5a5-a5a5a5a5a5a5"), new Guid("b5b5b5b5-b5b5-b5b5-b5b5-b5b5b5b5b5b5"), "CDR005", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Intermediate", true, new Guid("d5d5d5d5-d5d5-d5d5-d5d5-d5d5d5d5d5d5"), 150, 30, new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c1c1c1c1-c1c1-c1c1-c1c1-c1c1c1c1c1c1"), new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), "CDR001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Basic", true, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), 50, 10, new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2"), new Guid("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"), "CDR002", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Intermediate", true, new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), 100, 20, new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e3e3e3e3-e3e3-e3e3-e3e3-e3e3e3e3e3e3"), new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), "CDR003", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Advanced", true, new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"), 200, 50, new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f4f4f4f4-f4f4-f4f4-f4f4-f4f4f4f4f4f4"), new Guid("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4"), "CDR004", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Basic", true, new Guid("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4"), 60, 15, new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "UK__assembly_method_replica__slug",
                schema: "custome_design",
                table: "assembly_method_replicas",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__capability_replica__slug",
                schema: "custome_design",
                table: "capability_replicas",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_custom_design_assets_custom_design_request_id",
                schema: "custome_design",
                table: "custom_design_assets",
                column: "custom_design_request_id");

            migrationBuilder.CreateIndex(
                name: "UQ___custom_design_asset___code",
                schema: "custome_design",
                table: "custom_design_assets",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_custom_design_requests_custom_design_requirement_id",
                schema: "custome_design",
                table: "custom_design_requests",
                column: "custom_design_requirement_id");

            migrationBuilder.CreateIndex(
                name: "UQ___custom_design_request___code",
                schema: "custome_design",
                table: "custom_design_requests",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ___custom_design_requirement___code",
                schema: "custome_design",
                table: "custom_design_requirements",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__material_replica__slug",
                schema: "custome_design",
                table: "material_replicas",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ___requirement_capability_detail___requirement_id__capability_id",
                schema: "custome_design",
                table: "requirement_capability_details",
                columns: new[] { "custom_design_requirement_id", "capability_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK__topic_replica__slug",
                schema: "custome_design",
                table: "topic_replicas",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assembly_method_replicas",
                schema: "custome_design");

            migrationBuilder.DropTable(
                name: "capability_replicas",
                schema: "custome_design");

            migrationBuilder.DropTable(
                name: "custom_design_assets",
                schema: "custome_design");

            migrationBuilder.DropTable(
                name: "drive_replicas",
                schema: "custome_design");

            migrationBuilder.DropTable(
                name: "material_replicas",
                schema: "custome_design");

            migrationBuilder.DropTable(
                name: "requirement_capability_details",
                schema: "custome_design");

            migrationBuilder.DropTable(
                name: "topic_replicas",
                schema: "custome_design");

            migrationBuilder.DropTable(
                name: "custom_design_requests",
                schema: "custome_design");

            migrationBuilder.DropTable(
                name: "custom_design_requirements",
                schema: "custome_design");
        }
    }
}
