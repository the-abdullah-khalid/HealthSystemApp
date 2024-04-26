using HealthSystemApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace HealthSystemApp.CustomActionFilters.Authorization
{
    public class HierarchicalAccessControl : AuthorizationHandler<RoleRequirement>
    {
        private readonly ILogger<HierarchicalAccessControl> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly HealthSystemDbContext healthSystemDb;

        public HierarchicalAccessControl(ILogger<HierarchicalAccessControl> logger, IHttpContextAccessor httpContextAccessor,
            HealthSystemDbContext healthSystemDb)
        {
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            this.healthSystemDb = healthSystemDb;
        }

        private bool IsAuthorizedForHealthSystemRegionOrganization(string claimedId, string requestedRouteId)
        {
            return claimedId == requestedRouteId;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail(); // User is not authenticated, deny access
                await Task.CompletedTask;
                return;
            }

            var userRoles = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            var healthSystemIdClaim = context.User.FindFirst("HealthSystemId")?.Value; 
            var healthRegionIdClaim = context.User.FindFirst("HealthRegionId")?.Value;
            var organizationIdClaim = context.User.FindFirst("OrganizationId")?.Value;


            // Check if the user has the required role/s
            if (!userRoles.Any(role => requirement.AllowedRoles.Contains(role)))
            {
                context.Fail(); // User doesn't have any of the required roles, deny access
                await Task.CompletedTask;
                return;
            }


            if (userRoles.Contains("Administrator"))
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
                return;
            }
            else if (userRoles.Contains("HealthSystemAdmin"))
            {
                var idFound = httpContextAccessor.HttpContext.Request.RouteValues.TryGetValue("id", out var id);
                var IdFromRoute = idFound ? id?.ToString()?.ToLower() : null;


                //Check if the user has access to the requested Health System
                if (IsAuthorizedForHealthSystemRegionOrganization(healthSystemIdClaim, IdFromRoute))
                {
                    context.Succeed(requirement);
                    await Task.CompletedTask;
                    return;
                }

                var relationHealthSystemHealthRegions = await healthSystemDb.healthSystemHealthRegions.FirstOrDefaultAsync(r => r.HealthRegionId.ToString() == IdFromRoute);
                if (relationHealthSystemHealthRegions != null)
                {
                    var hsID = relationHealthSystemHealthRegions.HealthSystemId;
                    if (hsID.ToString() == healthSystemIdClaim)
                    {
                        context.Succeed(requirement);
                        await Task.CompletedTask;
                        return;
                    }
                }

                var relationHealthRegionsOrganizations = await healthSystemDb.healthRegionOrganizations.FirstOrDefaultAsync(org => org.OrganizationId.ToString() == IdFromRoute);
                if(relationHealthRegionsOrganizations != null)
                {
                    var hrID = relationHealthRegionsOrganizations.HealthRegionId;
                    var _relationHealthSystemHealthRegions = await healthSystemDb.healthSystemHealthRegions.FirstOrDefaultAsync(r => r.HealthRegionId.ToString() == hrID.ToString());
                    if (_relationHealthSystemHealthRegions != null)
                    {
                        var hsID = _relationHealthSystemHealthRegions.HealthSystemId;
                        if (hsID.ToString() == healthSystemIdClaim)
                        {
                            context.Succeed(requirement);
                            await Task.CompletedTask;
                            return;
                        }

                    }
                }

            }
            else if (userRoles.Contains("RegionAdmin"))
            {
                var idFound = httpContextAccessor.HttpContext.Request.RouteValues.TryGetValue("id", out var id);
                var IdFromRoute = idFound ? id?.ToString()?.ToLower() : null;


                //Check if the user has access to the requested Health Region
                if (IsAuthorizedForHealthSystemRegionOrganization(healthRegionIdClaim, IdFromRoute))
                {
                    context.Succeed(requirement);
                    await Task.CompletedTask;
                    return;
                }

                var relationHealthRegionsOrganizations = await healthSystemDb.healthRegionOrganizations.FirstOrDefaultAsync(org => org.OrganizationId.ToString() == IdFromRoute);
                if (relationHealthRegionsOrganizations != null)
                {
                    var hrID = relationHealthRegionsOrganizations.HealthRegionId;

                    if (hrID.ToString() == healthRegionIdClaim)
                    {
                        context.Succeed(requirement);
                        await Task.CompletedTask;
                        return;
                    }

                }

            }

            else if (userRoles.Contains("OrganizationAdmin"))
            {
                var idFound = httpContextAccessor.HttpContext.Request.RouteValues.TryGetValue("id", out var id);
                var IdFromRoute = idFound ? id?.ToString()?.ToLower() : null;


                //Check if the user has access to the requested Organization
                if (IsAuthorizedForHealthSystemRegionOrganization(organizationIdClaim, IdFromRoute))
                {
                    context.Succeed(requirement);
                    await Task.CompletedTask;
                    return;
                }
            }


            await Task.CompletedTask;
            return;
        }
    }
}
