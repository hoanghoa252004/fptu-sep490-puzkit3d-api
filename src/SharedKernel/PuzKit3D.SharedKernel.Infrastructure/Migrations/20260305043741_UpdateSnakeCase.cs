using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PuzKit3D.SharedKernel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.CreateTable(
                name: "identity_role",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identity_user",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    refresh_token = table.Column<string>(type: "text", nullable: true),
                    refresh_token_expiry_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    province_id = table.Column<string>(type: "text", nullable: true),
                    province_name = table.Column<string>(type: "text", nullable: true),
                    district_id = table.Column<string>(type: "text", nullable: true),
                    district_name = table.Column<string>(type: "text", nullable: true),
                    ward_code = table.Column<string>(type: "text", nullable: true),
                    ward_name = table.Column<string>(type: "text", nullable: true),
                    street_address = table.Column<string>(type: "text", nullable: true),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identity_role_claim",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_role_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_identity_role_claim_identity_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "identity",
                        principalTable: "identity_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_role_permission",
                schema: "identity",
                columns: table => new
                {
                    role_id = table.Column<string>(type: "text", nullable: false),
                    permission = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    granted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_role_permission", x => new { x.role_id, x.permission });
                    table.ForeignKey(
                        name: "fk_identity_user_role_permission_identity_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "identity",
                        principalTable: "identity_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_claim",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_identity_user_claim_identity_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "identity_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_login",
                schema: "identity",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_login", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_identity_user_login_identity_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "identity_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_permission",
                schema: "identity",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    permission = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    granted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    granted_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_permission", x => new { x.user_id, x.permission });
                    table.ForeignKey(
                        name: "fk_identity_user_permission_identity_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "identity_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_role",
                schema: "identity",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_role", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_identity_user_role_identity_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "identity",
                        principalTable: "identity_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_identity_user_role_identity_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "identity_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_token",
                schema: "identity",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_token", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_identity_user_token_identity_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "identity_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "identity_role",
                columns: new[] { "id", "concurrency_stamp", "created_at", "description", "name", "normalized_name" },
                values: new object[,]
                {
                    { "0b42c919-01c0-4109-ba04-d848c45dc413", null, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Low-level business access", "Business Manager", "BUSINESS_MANAGER" },
                    { "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", null, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "High-level business access", "Staff", "STAFF" },
                    { "9b7da615-9c41-4700-92a9-ca17337c5724", null, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Full system access", "System Administrator", "SYSTEM_ADMINISTRATOR" },
                    { "f634ede8-7091-48da-a969-2bf90ef86f2c", null, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Standard customer access", "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "identity_user",
                columns: new[] { "id", "access_failed_count", "concurrency_stamp", "created_at", "district_id", "district_name", "email", "email_confirmed", "first_name", "is_deleted", "last_name", "lockout_enabled", "lockout_end", "normalized_email", "normalized_user_name", "password_hash", "phone_number", "phone_number_confirmed", "province_id", "province_name", "refresh_token", "refresh_token_expiry_time", "security_stamp", "street_address", "two_factor_enabled", "updated_at", "user_name", "ward_code", "ward_name" },
                values: new object[,]
                {
                    { "admin-001", 0, "0aa95c6f-1688-4fea-a5c5-ba479dedca3e", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin@puzkit3d.com", true, "System", false, "Administrator", false, null, "ADMIN@PUZKIT3D.COM", "ADMIN@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEKIXCwrRDPT53/2gPTpIdDcQFbaPZqERErMypCwxXgBQ1SosnkQN46qeZovRhGap6g==", null, false, null, null, null, null, "d0498196-2ef2-47f4-9742-ee08346943e8", null, false, null, "admin@puzkit3d.com", null, null },
                    { "manager-001", 0, "b256c351-9300-45ba-b89d-9ae5beb1af11", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "manager@puzkit3d.com", true, "Business", false, "Manager", false, null, "MANAGER@PUZKIT3D.COM", "MANAGER@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEIIgwfsCVuEsoRQSZsffEdYn3HfxBvWgw7kjO0A4HozxMeGAa3U9qoXIo/IAy7Bkmg==", null, false, null, null, null, null, "501e0615-e07c-414d-ad8d-271846b77e6d", null, false, null, "manager@puzkit3d.com", null, null },
                    { "staff-001", 0, "5f2a88b3-471d-4014-adb6-434761a7472e", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "staff1@puzkit3d.com", true, "John", false, "Staff", false, null, "STAFF1@PUZKIT3D.COM", "STAFF1@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEKcQcFH5DLuATt2G873eZCq1aWuMsPLq3LXdBGpl8nY/IdGTKrhRbgVA6fSWHfFMKg==", null, false, null, null, null, null, "0322c039-49cc-48e6-816a-fb8e52153a18", null, false, null, "staff1@puzkit3d.com", null, null },
                    { "staff-002", 0, "1a6f3339-5371-4685-931f-ccc3329f5f99", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "staff2@puzkit3d.com", true, "Jane", false, "Staff", false, null, "STAFF2@PUZKIT3D.COM", "STAFF2@PUZKIT3D.COM", "AQAAAAIAAYagAAAAED6VVo1c9W7ggUpJn5NsOH13QrDO2YTIrQHIidtwJCUbI1btwqWWTYuiLF4w+74Q5A==", null, false, null, null, null, null, "c0c94a3e-fcda-47d4-bf32-2d702ac3b581", null, false, null, "staff2@puzkit3d.com", null, null },
                    { "staff-003", 0, "6eee882f-6908-47aa-93b9-ebe1299980b1", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "staff3@puzkit3d.com", true, "Mike", false, "Staff", false, null, "STAFF3@PUZKIT3D.COM", "STAFF3@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEGSGIQ+yUJjNXdTW7/8RrRaD20/7zrI8R6j3alEfmkNzYLi7EbjjC72wuxZPyC0kGQ==", null, false, null, null, null, null, "0ccc8350-6b6f-451a-a40c-187f7343400d", null, false, null, "staff3@puzkit3d.com", null, null }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "identity_user_role",
                columns: new[] { "role_id", "user_id" },
                values: new object[,]
                {
                    { "9b7da615-9c41-4700-92a9-ca17337c5724", "admin-001" },
                    { "0b42c919-01c0-4109-ba04-d848c45dc413", "manager-001" },
                    { "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", "staff-001" },
                    { "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", "staff-002" },
                    { "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", "staff-003" }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "identity_user_role_permission",
                columns: new[] { "permission", "role_id", "granted_at" },
                values: new object[,]
                {
                    { "catalog:assembly-methods:manage", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4850) },
                    { "catalog:assembly-methods:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4850) },
                    { "catalog:capabilities:manage", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4854) },
                    { "catalog:capabilities:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4854) },
                    { "catalog:materials:manage", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4853) },
                    { "catalog:materials:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4852) },
                    { "catalog:topics:manage", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4852) },
                    { "catalog:topics:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4851) },
                    { "instock:orders:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4856) },
                    { "instock:products:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4855) },
                    { "catalog:assembly-methods:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4859) },
                    { "catalog:assembly-methods:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4858) },
                    { "catalog:capabilities:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4862) },
                    { "catalog:capabilities:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4862) },
                    { "catalog:materials:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4861) },
                    { "catalog:materials:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4860) },
                    { "catalog:topics:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4860) },
                    { "catalog:topics:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4859) },
                    { "instock:orders:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4864) },
                    { "instock:products:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4863) },
                    { "users:create", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4822) },
                    { "users:delete", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4824) },
                    { "users:permissions:manage", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4825) },
                    { "users:roles:manage", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4824) },
                    { "users:update", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4823) },
                    { "users:view", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 3, 5, 4, 37, 41, 537, DateTimeKind.Utc).AddTicks(4816) }
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "identity",
                table: "identity_role",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_identity_role_claim_role_id",
                schema: "identity",
                table: "identity_role_claim",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "identity",
                table: "identity_user",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "identity",
                table: "identity_user",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_identity_user_claim_user_id",
                schema: "identity",
                table: "identity_user_claim",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_identity_user_login_user_id",
                schema: "identity",
                table: "identity_user_login",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_identity_user_role_role_id",
                schema: "identity",
                table: "identity_user_role",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "identity_role_claim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "identity_user_claim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "identity_user_login",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "identity_user_permission",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "identity_user_role",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "identity_user_role_permission",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "identity_user_token",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "identity_role",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "identity_user",
                schema: "identity");
        }
    }
}
