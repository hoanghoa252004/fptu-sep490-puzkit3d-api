using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Identity;

internal static class IdentityTableNames
{
    //internal const string Permissions = nameof(Permissions);

    internal const string ApplicationUser = "identity_user";
    internal const string ApplicationRole = "identity_role";
    internal const string ApplicationUserRole = "identity_user_role";

    internal const string ApplicationUserClaim = "identity_user_claim"; 
    internal const string ApplicationRoleClaim = "identity_role_claim";
    internal const string ApplicationUserLogin = "identity_user_login";
    internal const string ApplicationUserToken = "user_token";
}
