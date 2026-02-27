using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PuzKit3D.SharedKernel.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultUsersAndPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "identity",
                table: "identity_role",
                keyColumn: "Id",
                keyValue: "0b42c919-01c0-4109-ba04-d848c45dc413",
                column: "Name",
                value: "Business Manager");

            migrationBuilder.InsertData(
                schema: "identity",
                table: "identity_user",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { "admin-001", 0, "c27b680b-db09-453a-a51f-e16547e9c89e", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "admin@puzkit3d.com", true, "System", false, "Administrator", false, null, "ADMIN@PUZKIT3D.COM", "ADMIN@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEL5vCAeMvIYeaBmMmSCUIzwnoiMCIl2k2Ui0Ri71WT8S+2UKue4bx6OCW7hOdX/9Hg==", null, false, null, null, "f889c8a8-9adc-4dd8-921b-20e51c85ec95", false, null, "admin@puzkit3d.com" },
                    { "manager-001", 0, "55d6c47a-7b84-4323-822d-1bb2c90a5dec", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "manager@puzkit3d.com", true, "Business", false, "Manager", false, null, "MANAGER@PUZKIT3D.COM", "MANAGER@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEIDH4r6p4P5hrcggyaOn0wzB3ly50oKQz2QMFFZYhEGM26FLzleStudmP+4AcoXVhg==", null, false, null, null, "8240aeac-09b6-444d-aed7-fd6324e7303a", false, null, "manager@puzkit3d.com" },
                    { "staff-001", 0, "41a8de4e-b0cb-441d-a143-5344eac202af", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "staff1@puzkit3d.com", true, "John", false, "Staff", false, null, "STAFF1@PUZKIT3D.COM", "STAFF1@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEKVBQgs0NackpnAeDRXNXtpf+NfXIRwp/oIJMMF8XE8gRpxjoAFZ6Sb7/M/U47FJxw==", null, false, null, null, "c8d1c1f2-b067-4073-ba47-10db5d0b1522", false, null, "staff1@puzkit3d.com" },
                    { "staff-002", 0, "79b7c493-2dcb-47c4-8d89-a30e03e6d767", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "staff2@puzkit3d.com", true, "Jane", false, "Staff", false, null, "STAFF2@PUZKIT3D.COM", "STAFF2@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEKrKPbBSoaRBQJWGxBwgzAJKkXld8aTos/UFlNAf4/b2TKPWbMOoWei7noXKSY+M0A==", null, false, null, null, "01e381ab-8947-4dba-aae9-4975f29b3d87", false, null, "staff2@puzkit3d.com" },
                    { "staff-003", 0, "e9298e68-3b79-4d27-9421-86e1f40af162", new DateTime(2025, 2, 23, 0, 0, 0, 0, DateTimeKind.Utc), "staff3@puzkit3d.com", true, "Mike", false, "Staff", false, null, "STAFF3@PUZKIT3D.COM", "STAFF3@PUZKIT3D.COM", "AQAAAAIAAYagAAAAEMMzZY9YhyfUQhWYmsE6ZW9Q3jICuAyc/6eLtIs86ZFxlWgOeHx+L7VIgH1UBq16zg==", null, false, null, null, "b409f14c-5532-4501-80af-7a5159646d4b", false, null, "staff3@puzkit3d.com" }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "identity_user_role_permission",
                columns: new[] { "Permission", "RoleId", "GrantedAt" },
                values: new object[,]
                {
                    { "catalog:assembly-methods:manage", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1033) },
                    { "catalog:assembly-methods:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1033) },
                    { "catalog:capabilities:manage", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1037) },
                    { "catalog:capabilities:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1037) },
                    { "catalog:materials:manage", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1036) },
                    { "catalog:materials:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1035) },
                    { "catalog:topics:manage", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1035) },
                    { "catalog:topics:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1034) },
                    { "instock:orders:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1039) },
                    { "instock:products:view", "0b42c919-01c0-4109-ba04-d848c45dc413", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1038) },
                    { "catalog:assembly-methods:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1042) },
                    { "catalog:assembly-methods:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1041) },
                    { "catalog:capabilities:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1046) },
                    { "catalog:capabilities:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1045) },
                    { "catalog:materials:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1044) },
                    { "catalog:materials:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1044) },
                    { "catalog:topics:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1043) },
                    { "catalog:topics:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1042) },
                    { "instock:orders:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1047) },
                    { "instock:products:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(1047) },
                    { "users:create", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(994) },
                    { "users:delete", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(996) },
                    { "users:permissions:manage", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(997) },
                    { "users:roles:manage", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(996) },
                    { "users:update", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(995) },
                    { "users:view", "9b7da615-9c41-4700-92a9-ca17337c5724", new DateTime(2026, 2, 27, 7, 1, 14, 850, DateTimeKind.Utc).AddTicks(984) }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9b7da615-9c41-4700-92a9-ca17337c5724", "admin-001" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0b42c919-01c0-4109-ba04-d848c45dc413", "manager-001" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", "staff-001" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", "staff-002" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1a0d505f-46d8-4aaf-92c7-71ba90443dcb", "staff-003" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:assembly-methods:manage", "0b42c919-01c0-4109-ba04-d848c45dc413" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:assembly-methods:view", "0b42c919-01c0-4109-ba04-d848c45dc413" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:capabilities:manage", "0b42c919-01c0-4109-ba04-d848c45dc413" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:capabilities:view", "0b42c919-01c0-4109-ba04-d848c45dc413" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:materials:manage", "0b42c919-01c0-4109-ba04-d848c45dc413" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:materials:view", "0b42c919-01c0-4109-ba04-d848c45dc413" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:topics:manage", "0b42c919-01c0-4109-ba04-d848c45dc413" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:topics:view", "0b42c919-01c0-4109-ba04-d848c45dc413" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "instock:orders:view", "0b42c919-01c0-4109-ba04-d848c45dc413" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "instock:products:view", "0b42c919-01c0-4109-ba04-d848c45dc413" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:assembly-methods:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:assembly-methods:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:capabilities:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:capabilities:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:materials:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:materials:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:topics:manage", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "catalog:topics:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "instock:orders:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "instock:products:view", "1a0d505f-46d8-4aaf-92c7-71ba90443dcb" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "users:create", "9b7da615-9c41-4700-92a9-ca17337c5724" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "users:delete", "9b7da615-9c41-4700-92a9-ca17337c5724" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "users:permissions:manage", "9b7da615-9c41-4700-92a9-ca17337c5724" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "users:roles:manage", "9b7da615-9c41-4700-92a9-ca17337c5724" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "users:update", "9b7da615-9c41-4700-92a9-ca17337c5724" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user_role_permission",
                keyColumns: new[] { "Permission", "RoleId" },
                keyValues: new object[] { "users:view", "9b7da615-9c41-4700-92a9-ca17337c5724" });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user",
                keyColumn: "Id",
                keyValue: "admin-001");

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user",
                keyColumn: "Id",
                keyValue: "manager-001");

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user",
                keyColumn: "Id",
                keyValue: "staff-001");

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user",
                keyColumn: "Id",
                keyValue: "staff-002");

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "identity_user",
                keyColumn: "Id",
                keyValue: "staff-003");

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "identity_role",
                keyColumn: "Id",
                keyValue: "0b42c919-01c0-4109-ba04-d848c45dc413",
                column: "Name",
                value: "Business Manager ");
        }
    }
}
