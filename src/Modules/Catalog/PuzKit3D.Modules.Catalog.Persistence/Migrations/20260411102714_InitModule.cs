using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                    quantity_in_stock = table.Column<int>(type: "integer", nullable: false),
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
                    capability_id = table.Column<Guid>(type: "uuid", nullable: false),
                    drive_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_capability_drives", x => new { x.capability_id, x.drive_id });
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

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "assembly_methods",
                columns: new[] { "id", "created_at", "description", "factor_percentage", "is_active", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly using tight fitting pieces", 1.15m, true, "Friction Fit", "friction-fit", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b5b5b5b5-b5b5-b5b5-b5b5-b5b5b5b5b5b5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly using magnetic connections", 1.4m, true, "Magnetic Assembly", "magnetic-assembly", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Easy snap assembly without tools", 1.1m, true, "Snap-Fit", "snap-fit", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly using adhesive bonding", 1.0m, true, "Glue Assembly", "glue-assembly", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Assembly using screws and bolts", 1.3m, true, "Screw Assembly", "screw-assembly", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "capabilities",
                columns: new[] { "id", "created_at", "description", "factor_percentage", "is_active", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Model operated by manual movement", 1.3m, true, "Manual Movement", "manual-movement", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Model with LED lighting effects", 1.4m, true, "LED Light Feature", "led-light-feature", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Model with musical features via rotating mechanism", 1.6m, true, "Musical Gear", "musical-gear", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Static model for display only", 1.0m, true, "Static Display", "static-display", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Model with electric motor-powered movement", 1.8m, true, "Move with Motor", "move-with-motor", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "drives",
                columns: new[] { "id", "created_at", "description", "is_active", "min_volume", "name", "quantity_in_stock", "updated_at" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "No drive mechanism", true, null, "None", 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Electric motor drive", true, 50, "Motor", 100, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mechanical gear system", true, 30, "Gearbox", 150, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LED lighting system", true, 10, "LED Module", 200, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mechanical music box", true, 50, "Music Box", 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "materials",
                columns: new[] { "id", "base_price", "created_at", "description", "factor_percentage", "is_active", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), 2000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Durable plastic material", 0.85m, true, "Plastic", "plastic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"), 9000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Premium metal components", 1.6m, true, "Metal", "metal", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4"), 1000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Eco-friendly cardboard material", 0.7m, true, "Cardboard", "cardboard", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d5d5d5d5-d5d5-d5d5-d5d5-d5d5d5d5d5d5"), 7000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Advanced composite materials", 1.3m, true, "Composite", "composite", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), 4000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Natural wood material", 1.0m, true, "Wood", "wood", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "topics",
                columns: new[] { "id", "created_at", "description", "factor_percentage", "is_active", "name", "parent_id", "slug", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a1a1a1a1-0001-0001-0001-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lions, tigers and other wild animals", 1.2m, true, "Wild Animals", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), "wild-animals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1a1a1a1-0002-0002-0002-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cats, dogs and domestic animals", 1.0m, true, "Pets", new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), "pets", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Animal themed 3D puzzles", 1.1m, true, "Animals", null, "animals", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b2b2b2b2-0001-0001-0001-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Modern and classic cars", 1.2m, true, "Cars", new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), "cars", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b2b2b2b2-0002-0002-0002-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Airplanes and helicopters", 1.4m, true, "Aircraft", new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), "aircraft", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vehicle and transportation themed puzzles", 1.3m, true, "Vehicles", null, "vehicles", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c3c3c3c3-0001-0001-0001-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Famous world landmarks", 1.6m, true, "Landmarks", new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"), "landmarks", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c3c3c3c3-0002-0002-0002-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Residential buildings and houses", 1.3m, true, "Houses", new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"), "houses", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Famous buildings and landmarks", 1.5m, true, "Architecture", null, "architecture", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d4d4d4d4-0001-0001-0001-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mountain landscapes", 1.1m, true, "Mountains", new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"), "mountains", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d4d4d4d4-0002-0002-0002-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ocean and sea environments", 1.2m, true, "Oceans", new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"), "oceans", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Natural landscapes and scenery", 1.0m, true, "Nature", null, "nature", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e5e5e5e5-0001-0001-0001-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dragon creatures and models", 1.8m, true, "Dragons", new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"), "dragons", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e5e5e5e5-0002-0002-0002-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fantasy magical environments", 1.7m, true, "Magic Worlds", new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"), "magic-worlds", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fantasy creatures and magical worlds", 1.6m, true, "Fantasy", null, "fantasy", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "capability_drives",
                columns: new[] { "capability_id", "drive_id" },
                values: new object[,]
                {
                    { new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), new Guid("33333333-3333-3333-3333-333333333333") }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "capability_material_assemblies",
                columns: new[] { "id", "assembly_id", "assembly_method_id", "capability_id", "is_active", "material_id" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-000000000001"), new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), null, new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), true, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1") },
                    { new Guid("90000000-0000-0000-0000-000000000002"), new Guid("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"), null, new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), true, new Guid("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4") },
                    { new Guid("90000000-0000-0000-0000-000000000003"), new Guid("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4"), null, new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"), true, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1") },
                    { new Guid("90000000-0000-0000-0000-000000000004"), new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), null, new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"), true, new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2") },
                    { new Guid("90000000-0000-0000-0000-000000000005"), new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), null, new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), true, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1") },
                    { new Guid("90000000-0000-0000-0000-000000000006"), new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), null, new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), true, new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2") },
                    { new Guid("90000000-0000-0000-0000-000000000007"), new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), null, new Guid("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4"), true, new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2") },
                    { new Guid("90000000-0000-0000-0000-000000000008"), new Guid("b5b5b5b5-b5b5-b5b5-b5b5-b5b5b5b5b5b5"), null, new Guid("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4"), true, new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3") },
                    { new Guid("90000000-0000-0000-0000-000000000009"), new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), null, new Guid("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5"), true, new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3") },
                    { new Guid("90000000-0000-0000-0000-000000000010"), new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), null, new Guid("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5"), true, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1") }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "topic_material_capabilities",
                columns: new[] { "id", "capability_id", "is_active", "material_id", "topic_id" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-000000000001"), new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), true, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1") },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"), true, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1") },
                    { new Guid("80000000-0000-0000-0000-000000000003"), new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), true, new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2") },
                    { new Guid("80000000-0000-0000-0000-000000000004"), new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), true, new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"), new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2") },
                    { new Guid("80000000-0000-0000-0000-000000000005"), new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), true, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3") },
                    { new Guid("80000000-0000-0000-0000-000000000006"), new Guid("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4"), true, new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"), new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3") },
                    { new Guid("80000000-0000-0000-0000-000000000007"), new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), true, new Guid("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4"), new Guid("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4") },
                    { new Guid("80000000-0000-0000-0000-000000000008"), new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), true, new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"), new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5") },
                    { new Guid("80000000-0000-0000-0000-000000000009"), new Guid("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4"), true, new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"), new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5") },
                    { new Guid("80000000-0000-0000-0000-000000000010"), new Guid("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5"), true, new Guid("d5d5d5d5-d5d5-d5d5-d5d5-d5d5d5d5d5d5"), new Guid("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5") }
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
