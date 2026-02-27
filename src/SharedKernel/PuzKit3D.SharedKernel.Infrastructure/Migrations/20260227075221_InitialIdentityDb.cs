using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PuzKit3D.SharedKernel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentityDb : Migration
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
                    Id = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_user",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_role_claim",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_role_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_identity_role_claim_identity_role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "identity_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_role_permission",
                schema: "identity",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    Permission = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_user_role_permission", x => new { x.RoleId, x.Permission });
                    table.ForeignKey(
                        name: "FK_identity_user_role_permission_identity_role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "identity_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_claim",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_user_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_identity_user_claim_identity_user_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "identity_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_login",
                schema: "identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_user_login", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_identity_user_login_identity_user_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "identity_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_permission",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Permission = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GrantedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_user_permission", x => new { x.UserId, x.Permission });
                    table.ForeignKey(
                        name: "FK_identity_user_permission_identity_user_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "identity_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_role",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_user_role", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_identity_user_role_identity_role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "identity_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_identity_user_role_identity_user_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "identity_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_token",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_user_token", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_identity_user_token_identity_user_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "identity_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "identity_role",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "Description", "Name", "NormalizedName" },
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
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { "admin-001", 0, "1d5366a7-43f2-405a-a85b-1c913c0c5b2f", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "admin@puzkit3d.com", true, "System", false, "Administrator", false, null, "ADMIN@PUZKIT3D.COM", "ADMIN@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEPSfvsSKDzPCnkk8Mq3OFqUzOSemt4JjQU4Bg7a8oqcYFDNEuMFUlcClo72Obq8t9Q==", null, false, null, null, "52829083-e3e6-4328-bc06-ffa922a0abbc", false, null, "admin@puzkit3d.com" },
                    { "manager-001", 0, "cc327c3a-bdcc-4b95-bce3-65177dfbe4fe", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "manager@puzkit3d.com", true, "Business", false, "Manager", false, null, "MANAGER@PUZKIT3D.COM", "MANAGER@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEDr0+IKdspm90K/Atw4QgWL7d72tlODQpuwXZJ4Vy5bpkB8K18GAeCpelTCsWnQRoQ==", null, false, null, null, "6ffb607d-9395-49c2-9625-ed93885994b0", false, null, "manager@puzkit3d.com" },
                    { "staff-001", 0, "9af8665d-1aca-4763-b814-9a25da7bd6fc", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "staff1@puzkit3d.com", true, "John", false, "Staff", false, null, "STAFF1@PUZKIT3D.COM", "STAFF1@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEBIUHn2jIxsukbuuIsZVWDp/AJ4+374ehGUHJh4zxk91WeYH7VWljn3r6uFTJ392dA==", null, false, null, null, "2ae9876a-6240-4581-a894-21133bbbbe8c", false, null, "staff1@puzkit3d.com" },
                    { "staff-002", 0, "d693b4f9-d518-4fb0-92db-b61633590dad", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "staff2@puzkit3d.com", true, "Jane", false, "Staff", false, null, "STAFF2@PUZKIT3D.COM", "STAFF2@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEIRgv1XVHWifHGFVB26ohtzsahQj3oY9/gGGUcZJ9Du1Akp0nTMNc8rB8q1v/GLBeQ==", null, false, null, null, "24030a5a-edc4-44a5-8dea-db68be5fe6d7", false, null, "staff2@puzkit3d.com" },
                    { "staff-003", 0, "5bd11bf3-e42b-4533-904a-e3558e15240f", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "staff3@puzkit3d.com", true, "Mike", false, "Staff", false, null, "STAFF3@PUZKIT3D.COM", "STAFF3@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEDAZhxLEKyKNG+d5zJ2FOyGFXJPnM/uTev/GSjw6I0qVbidgJtfMl8SORarbMZc9LA==", null, false, null, null, "10c189c9-c1bf-45a8-9c04-33f30aef2bc0", false, null, "staff3@puzkit3d.com" }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "identity_user_role",
                columns: new[] { "RoleId", "UserId" },
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
                columns: new[] { "Permission", "RoleId", "GrantedAt" },
                values: new object[,]
                {
                    { "catalog:assembly-methods:manage", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7837) },
                    { "catalog:assembly-methods:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7837) },
                    { "catalog:capabilities:manage", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7841) },
                    { "catalog:capabilities:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7840) },
                    { "catalog:materials:manage", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7840) },
                    { "catalog:materials:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7839) },
                    { "catalog:topics:manage", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7839) },
                    { "catalog:topics:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7838) },
                    { "instock:orders:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7842) },
                    { "instock:products:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7841) },
                    { "catalog:assembly-methods:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7845) },
                    { "catalog:assembly-methods:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7844) },
                    { "catalog:capabilities:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7848) },
                    { "catalog:capabilities:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7848) },
                    { "catalog:materials:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7847) },
                    { "catalog:materials:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7846) },
                    { "catalog:topics:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7846) },
                    { "catalog:topics:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7845) },
                    { "instock:orders:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7938) },
                    { "instock:products:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7849) },
                    { "users:create", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7805) },
                    { "users:delete", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7806) },
                    { "users:permissions:manage", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7807) },
                    { "users:roles:manage", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7807) },
                    { "users:update", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7805) },
                    { "users:view", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 2, 27, 7, 52, 19, 160, DateTimeKind.Utc).AddTicks(7799) }
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "identity",
                table: "identity_role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identity_role_claim_RoleId",
                schema: "identity",
                table: "identity_role_claim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "identity",
                table: "identity_user",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "identity",
                table: "identity_user",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identity_user_claim_UserId",
                schema: "identity",
                table: "identity_user_claim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_identity_user_login_UserId",
                schema: "identity",
                table: "identity_user_login",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_identity_user_role_RoleId",
                schema: "identity",
                table: "identity_user_role",
                column: "RoleId");
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
