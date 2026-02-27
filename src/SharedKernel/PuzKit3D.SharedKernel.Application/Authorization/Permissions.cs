using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Authorization;

/// <summary>
/// Static class containing all application permissions
/// </summary>
public static class Permissions
{
    // Format: "module:resource:action"

    public static class InStock
    {
        public const string ViewProducts = "instock:products:view";
        public const string CreateProduct = "instock:products:create";
        public const string UpdateProduct = "instock:products:update";
        public const string DeleteProduct = "instock:products:delete";

        public const string ViewOrders = "instock:orders:view";
        public const string CreateOrder = "instock:orders:create";
        public const string UpdateOrder = "instock:orders:update";
        public const string CancelOrder = "instock:orders:cancel";
    }

    public static class PartnerOrdering
    {
        public const string ViewPartners = "partner:partners:view";
        public const string ManagePartners = "partner:partners:manage";
        public const string ViewOrders = "partner:orders:view";
        public const string ProcessOrders = "partner:orders:process";
    }

    public static class Catalog
    {
        public const string ViewAssemblyMethods = "catalog:assembly-methods:view";
        public const string ManageAssemblyMethods = "catalog:assembly-methods:manage";
        
        public const string ViewTopics = "catalog:topics:view";
        public const string ManageTopics = "catalog:topics:manage";
        
        public const string ViewMaterials = "catalog:materials:view";
        public const string ManageMaterials = "catalog:materials:manage";
        
        public const string ViewCapabilities = "catalog:capabilities:view";
        public const string ManageCapabilities = "catalog:capabilities:manage";
    }

    public static class Users
    {
        public const string ViewUsers = "users:view";
        public const string CreateUser = "users:create";
        public const string UpdateUser = "users:update";
        public const string DeleteUser = "users:delete";
        public const string ManageRoles = "users:roles:manage";
        public const string ManagePermissions = "users:permissions:manage";
    }

    /// <summary>
    /// Gets all defined permissions
    /// </summary>
    public static IEnumerable<string> GetAllPermissions()
    {
        return typeof(Permissions)
            .GetNestedTypes()
            .SelectMany(t => t.GetFields())
            .Select(f => f.GetValue(null)?.ToString())
            .Where(v => v is not null)
            .Cast<string>();
    }
}
