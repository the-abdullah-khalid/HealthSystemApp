using HealthSystemApp.Data;
using HealthSystemApp.DTOs.AuthenticationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;

namespace HealthSystemApp.CustomActionFilters.Authorization
{
    public class HierarchicalAccessControl : AuthorizationHandler<RoleRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly HealthSystemDbContext healthSystemDb;

        public HierarchicalAccessControl(IHttpContextAccessor httpContextAccessor,
            HealthSystemDbContext healthSystemDb)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.healthSystemDb = healthSystemDb;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            httpContextAccessor.HttpContext.Request.EnableBuffering();
            var requestBody = await new StreamReader(httpContextAccessor.HttpContext.Request.Body).ReadToEndAsync();

            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail(); // User is not authenticated, deny access
                await Task.CompletedTask;
                return;
            }

            var userRoles = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();




            // Check if the user has the required role/s
            if (!userRoles.Any(role => requirement.AllowedRoles.Contains(role)))
            {
                context.Fail(); // User doesn't have any of the required roles, deny access
                await Task.CompletedTask;
                return;
            }

            var claimedIds = context.User.FindAll("ClaimedId").Select(c => c.Value).ToList();

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

                var registerRequestDto = JsonConvert.DeserializeObject<RegisterRequestDTO>(requestBody);
                if (httpContextAccessor.HttpContext.Request.Body.CanSeek)
                    httpContextAccessor.HttpContext.Request.Body.Position = 0;


                if (registerRequestDto != null && claimedIds != null)
                {
                    foreach (var claimedId in claimedIds)
                    {
                        if (await IsAuthorizedForCreatingNewHealthRegionUser(claimedId, registerRequestDto))
                        {
                            context.Succeed(requirement);
                            await Task.CompletedTask;
                            return;
                        }
                        else if (await IsHSAdminAuthorizedForCreatingNewOrganizationUser(claimedId, registerRequestDto))
                        {
                            context.Succeed(requirement);
                            await Task.CompletedTask;
                            return;
                        }

                    }
                }

                //Check if the user has access to the requested Health System
                foreach (var claimedId in claimedIds)
                {
                    if (IsAuthorizedForHealthSystemRegionOrganization(claimedId, IdFromRoute))
                    {
                        context.Succeed(requirement);
                        await Task.CompletedTask;
                        return;
                    }
                }


                foreach (var claimedId in claimedIds)
                {
                    var relationHealthSystemHealthRegions = await healthSystemDb.healthSystemHealthRegions.FirstOrDefaultAsync(r => r.HealthRegionId.ToString() == IdFromRoute);
                    if (relationHealthSystemHealthRegions != null)
                    {
                        var hsID = relationHealthSystemHealthRegions.HealthSystemId;
                        if (hsID.ToString() == claimedId)
                        {
                            context.Succeed(requirement);
                            await Task.CompletedTask;
                            return;
                        }
                    }
                }

                foreach (var claimedId in claimedIds)
                {
                    var relationHealthRegionsOrganizations = await healthSystemDb.healthRegionOrganizations.FirstOrDefaultAsync(org => org.OrganizationId.ToString() == IdFromRoute);
                    if (relationHealthRegionsOrganizations != null)
                    {
                        var hrID = relationHealthRegionsOrganizations.HealthRegionId;
                        var _relationHealthSystemHealthRegions = await healthSystemDb.healthSystemHealthRegions.FirstOrDefaultAsync(r => r.HealthRegionId.ToString() == hrID.ToString());
                        if (_relationHealthSystemHealthRegions != null)
                        {
                            var hsID = _relationHealthSystemHealthRegions.HealthSystemId;
                            if (hsID.ToString() == claimedId)
                            {
                                context.Succeed(requirement);
                                await Task.CompletedTask;
                                return;
                            }

                        }
                    }
                }

            }

            else if (userRoles.Contains("RegionAdmin"))
            {
                var idFound = httpContextAccessor.HttpContext.Request.RouteValues.TryGetValue("id", out var id);
                var IdFromRoute = idFound ? id?.ToString()?.ToLower() : null;


                var registerRequestDto = JsonConvert.DeserializeObject<RegisterRequestDTO>(requestBody);
                if (httpContextAccessor.HttpContext.Request.Body.CanSeek)
                    httpContextAccessor.HttpContext.Request.Body.Position = 0;

                if (registerRequestDto != null && claimedIds != null)
                {
                    foreach (var claimedId in claimedIds)
                    {
                        if (await IsHRAdminAuthorizedForCreatingNewOrganizationUser(claimedId, registerRequestDto))
                        {
                            context.Succeed(requirement);
                            await Task.CompletedTask;
                            return;
                        }
                    }
                }

                //Check if the user has access to the requested Health Region
                foreach (var claimedId in claimedIds)
                {
                    if (IsAuthorizedForHealthSystemRegionOrganization(claimedId, IdFromRoute))
                    {
                        context.Succeed(requirement);
                        await Task.CompletedTask;
                        return;
                    }
                }

                foreach (var claimedId in claimedIds)
                {
                    var relationHealthRegionsOrganizations = await healthSystemDb.healthRegionOrganizations.FirstOrDefaultAsync(org => org.OrganizationId.ToString() == IdFromRoute);
                    if (relationHealthRegionsOrganizations != null)
                    {
                        var hrID = relationHealthRegionsOrganizations.HealthRegionId;

                        if (hrID.ToString() == claimedId)
                        {
                            context.Succeed(requirement);
                            await Task.CompletedTask;
                            return;
                        }

                    }
                }

            }

            else if (userRoles.Contains("OrganizationAdmin"))
            {
                var idFound = httpContextAccessor.HttpContext.Request.RouteValues.TryGetValue("id", out var id);
                var IdFromRoute = idFound ? id?.ToString()?.ToLower() : null;


                //Check if the user has access to the requested Organization
                foreach (var claimedId in claimedIds)
                {
                    if (IsAuthorizedForHealthSystemRegionOrganization(claimedId, IdFromRoute))
                    {
                        context.Succeed(requirement);
                        await Task.CompletedTask;
                        return;
                    }
                }
            }


            await Task.CompletedTask;
            return;
        }
        private bool IsAuthorizedForHealthSystemRegionOrganization(string claimedId, string requestedRouteId)
        {
            return claimedId == requestedRouteId;
        }

        private async Task<bool> IsAuthorizedForCreatingNewHealthRegionUser(string claimedId, RegisterRequestDTO registerRequestDTO)
        {
            if (registerRequestDTO.Role.Contains("RegionAdmin"))
            {
                foreach (var claimId in registerRequestDTO.ClaimIds)
                {
                    var HsID = await healthSystemDb.healthSystemHealthRegions.FindAsync(claimId, Guid.Parse(claimedId));
                    if (HsID == null)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private async Task<bool> IsHSAdminAuthorizedForCreatingNewOrganizationUser(string claimedId, RegisterRequestDTO registerRequestDTO)
        {
            if ((registerRequestDTO.Role.Contains("OrganizationAdmin")))
            {
                foreach (var claimId in registerRequestDTO.ClaimIds)
                {
                    var HrID = await healthSystemDb.healthRegionOrganizations.FirstOrDefaultAsync(org => org.OrganizationId == claimId);
                    if (HrID == null)
                    {
                        return false;
                    }

                    var HsID = await healthSystemDb.healthSystemHealthRegions.FindAsync(HrID.HealthRegionId, Guid.Parse(claimedId));
                    if (HsID == null)
                    {
                        return false;
                    }

                }
                return true;
            }


            return false;
        }

        private async Task<bool> IsHRAdminAuthorizedForCreatingNewOrganizationUser(string claimedId, RegisterRequestDTO registerRequestDTO)
        {
            if ((registerRequestDTO.Role.Contains("OrganizationAdmin")))
            {

                foreach (var claimId in registerRequestDTO.ClaimIds)
                {
                    var HrID = await healthSystemDb.healthRegionOrganizations.FirstOrDefaultAsync(org => org.OrganizationId == claimId);
                    if (HrID == null)
                    {
                        return false;
                    }

                    if (HrID.HealthRegionId.ToString() != claimedId)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

      
    }
}
