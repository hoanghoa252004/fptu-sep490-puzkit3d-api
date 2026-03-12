using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PuzKit3D.SharedKernel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitModule : Migration
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
                    { "0b42c919-01c0-4109-ba04-d848c45dc413", null, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "High-level business access", "Business Manager", "BUSINESS MANAGER" },
                    { "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", null, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Low-level business access", "Staff", "STAFF" },
                    { "9b7da615-9c41-4700-92a9-ca17337c5724", null, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Full system access", "System Administrator", "SYSTEM ADMINISTRATOR" },
                    { "f634ede8-7091-48da-a969-2bf90ef86f2c", null, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Standard customer access", "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "identity_user",
                columns: new[] { "id", "access_failed_count", "concurrency_stamp", "created_at", "district_id", "district_name", "email", "email_confirmed", "first_name", "is_deleted", "last_name", "lockout_enabled", "lockout_end", "normalized_email", "normalized_user_name", "password_hash", "phone_number", "phone_number_confirmed", "province_id", "province_name", "refresh_token", "refresh_token_expiry_time", "security_stamp", "street_address", "two_factor_enabled", "updated_at", "user_name", "ward_code", "ward_name" },
                values: new object[,]
                {
                    { "10fa5863-e39c-4876-856e-5c4cfbd321dc", 0, "C3D4E5F6-A7B8-9012-CDEF-345678901234", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "staff@puzkit3d.com", true, "PuzKit3D", false, "Staff", false, null, "STAFF@PUZKIT3D.COM", "STAFF@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEAQBI/6CDIDKs6E2P9Jqq+Atq/6Zq2aDRu8PxMKeI0+UUSYkpHbTZ3SM9idZCHc3FA==", null, false, null, null, null, null, "C7D8E9F0-1A2B-3C4D-5E6F-7A8B9C0D1E2F", null, false, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "staff@puzkit3d.com", null, null },
                    { "15e5d4ac-a548-4f8a-9846-5fddc79c79c2", 0, "B2C3D4E5-F6A7-8901-BCDE-F23456789012", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "manager@puzkit3d.com", true, "PuzKit3D", false, "Business Manager", false, null, "MANAGER@PUZKIT3D.COM", "MANAGER@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEBvw5howPTqR3gzQNDHfMxb3xy9CjcMnJS0doCEymxAZb3xS5bbBXREi141FvIkNvQ==", null, false, null, null, null, null, "B6E7F8A9-1C2D-3E4F-5A6B-7C8D9E0F1A2B", null, false, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "manager@puzkit3d.com", null, null },
                    { "21d3261d-9ab4-45b9-b6cd-fba0231d285c", 0, "D4E5F6A7-B8C9-0123-DEF4-567890123456", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "customer@puzkit3d.com", true, "PuzKit3D", false, "Customer", false, null, "CUSTOMER@PUZKIT3D.COM", "CUSTOMER@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEJlVh5MVBSxyJvjVCzdIk35eavJteEJN4dBIe0wq3hh6GbAAc1xleP40pg4KcJqBbA==", null, false, null, null, null, null, "D8E9F0A1-2B3C-4D5E-6F7A-8B9C0D1E2F3A", null, false, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "customer@puzkit3d.com", null, null },
                    { "71ac7ce7-84e2-44f6-8765-209244d0cbb3", 0, "A1B2C3D4-E5F6-7890-ABCD-EF1234567890", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin@puzkit3d.com", true, "PuzKit3D", false, "System Administrator", false, null, "ADMIN@PUZKIT3D.COM", "ADMIN@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEJL8OfPfS/aR23An303wsHONeCA+gxCoIb3H6jaEdJgz2W0/FTTCIlF+5X0VXN+tDg==", null, false, null, null, null, null, "D5F8E9A1-2B3C-4D5E-6F7A-8B9C0D1E2F3A", null, false, new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "admin@puzkit3d.com", null, null }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "identity_user_role",
                columns: new[] { "role_id", "user_id" },
                values: new object[,]
                {
                    { "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", "10fa5863-e39c-4876-856e-5c4cfbd321dc" },
                    { "0b42c919-01c0-4109-ba04-d848c45dc413", "15e5d4ac-a548-4f8a-9846-5fddc79c79c2" },
                    { "f634ede8-7091-48da-a969-2bf90ef86f2c", "21d3261d-9ab4-45b9-b6cd-fba0231d285c" },
                    { "9b7da615-9c41-4700-92a9-ca17337c5724", "71ac7ce7-84e2-44f6-8765-209244d0cbb3" }
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
                name: "identity_user_role",
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
