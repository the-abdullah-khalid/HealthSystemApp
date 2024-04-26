using Microsoft.AspNetCore.Authorization;

namespace HealthSystemApp.CustomActionFilters.Authorization
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public List<string> AllowedRoles { get; }

        public RoleRequirement(List<string> roles)
        {
            AllowedRoles = roles;
        }
    }

}
