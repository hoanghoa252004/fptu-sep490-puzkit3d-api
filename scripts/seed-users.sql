-- Script SQL ?? seed test users tr?c ti?p vào database
-- Password hash ???c t?o b?i ASP.NET Identity PasswordHasher
-- Admin password: Admin123!@#
-- User password: User123!@#

BEGIN;

-- Xóa user c? n?u có (?? tránh conflict)
DELETE FROM identity.identity_user WHERE "Email" IN ('admin@puzkit3d.com', 'user@puzkit3d.com');

-- Insert Admin User
-- Password: Admin123!@# 
-- Hash ???c t?o b?i ASP.NET Core Identity PasswordHasher V3
INSERT INTO identity.identity_user (
    "Id", 
    "UserName", 
    "NormalizedUserName", 
    "Email", 
    "NormalizedEmail", 
    "EmailConfirmed", 
    "PasswordHash",
    "SecurityStamp",
    "ConcurrencyStamp",
    "PhoneNumberConfirmed",
    "TwoFactorEnabled",
    "LockoutEnabled",
    "AccessFailedCount",
    "FirstName",
    "LastName",
    "CreatedAt",
    "IsDeleted"
) VALUES (
    gen_random_uuid()::text,
    'admin@puzkit3d.com',
    'ADMIN@PUZKIT3D.COM',
    'admin@puzkit3d.com',
    'ADMIN@PUZKIT3D.COM',
    true,
    'AQAAAAIAAYagAAAAEGMxvwJxK8qYHZFxvZQxLwYxqZ0vJ6F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5==',
    gen_random_uuid()::text,
    gen_random_uuid()::text,
    false,
    false,
    true,
    0,
    'Admin',
    'User',
    NOW(),
    false
) RETURNING "Id" AS admin_user_id;

-- Insert Regular User
-- Password: User123!@#
INSERT INTO identity.identity_user (
    "Id", 
    "UserName", 
    "NormalizedUserName", 
    "Email", 
    "NormalizedEmail", 
    "EmailConfirmed", 
    "PasswordHash",
    "SecurityStamp",
    "ConcurrencyStamp",
    "PhoneNumberConfirmed",
    "TwoFactorEnabled",
    "LockoutEnabled",
    "AccessFailedCount",
    "FirstName",
    "LastName",
    "CreatedAt",
    "IsDeleted"
) VALUES (
    gen_random_uuid()::text,
    'user@puzkit3d.com',
    'USER@PUZKIT3D.COM',
    'user@puzkit3d.com',
    'USER@PUZKIT3D.COM',
    true,
    'AQAAAAIAAYagAAAAEGMxvwJxK8qYHZFxvZQxLwYxqZ0vJ6F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5==',
    gen_random_uuid()::text,
    gen_random_uuid()::text,
    false,
    false,
    true,
    0,
    'Test',
    'User',
    NOW(),
    false
) RETURNING "Id" AS regular_user_id;

-- Assign roles (using the role IDs from migration seed)
-- Note: This will only work if you know the user IDs, so we'll do this differently

COMMIT;

-- Display created users
SELECT "Email", "FirstName", "LastName", "EmailConfirmed" 
FROM identity.identity_user 
WHERE "Email" IN ('admin@puzkit3d.com', 'user@puzkit3d.com');

-- Note: Password hash ? trên là m?u và s? KHÔNG HO?T ??NG
-- C?n s? d?ng script PowerShell bên d??i ?? seed ?úng
