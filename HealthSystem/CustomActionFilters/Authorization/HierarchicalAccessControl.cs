using HealthSystemApp.Data;
using HealthSystemApp.DTOs.AuthenticationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;

namespace HealthSystemApp.CustomActionFilters.Authorization
{
    public class HierarchicalAccessControl : AuthorizationHandler<RoleRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly HealthSystemDbContext healthSystemDb;
        private readonly HealthSystemAuthDbContext healthSystemAuthDbContext;

        public HierarchicalAccessControl(IHttpContextAccessor httpContextAccessor,
            HealthSystemDbContext healthSystemDb,HealthSystemAuthDbContext healthSystemAuthDbContext)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.healthSystemDb = healthSystemDb;
            this.healthSystemAuthDbContext = healthSystemAuthDbContext;
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
                if (httpContextAccessor.HttpContext.Request.Body.CanSeek)
                    httpContextAccessor.HttpContext.Request.Body.Position = 0;

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
        private async Task<bool> IsAuthorizedToEditUserDetails(Guid loggedInUserId,Guid UserId,UpdateUserRequestDTO updateUserRequestDTO)
        {
            if(updateUserRequestDTO.Role.Contains("HealthRegionAdmin"))
            {
                //in future i will make a stored procdeure for it
                var userClaims =await healthSystemAuthDbContext
                    .UserRoles
                    .Where(user => user.UserId == UserId.ToString())
                    .Select(user => user.ClaimedId)
                    .ToListAsync();

                ////if already present user claims=>healthsystem matches the healthsystemId of loggedInUser , then only
                //List<Guid?> healthSystems = await  healthSystemDb
                //     .healthSystemHealthRegions
                //     .Where(region => userClaims.Contains(region.HealthRegionId))
                //     .Select(region => region.HealthSystemId)
                //     .Distinct()
                //     .ToListAsync();
                //if(healthSystems.Count=1)

            }
            else if(updateUserRequestDTO.Role.Contains("OrganizationAdmin"))
            {
                
            }
            return false;
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
                    Guid? HrID = await healthSystemDb.healthRegionOrganizations
                    .Where(org => org.OrganizationId == claimId)
                    .Select(org => org.HealthRegionId)
                    .FirstOrDefaultAsync();

                    if (HrID == null)
                    {
                        return false;
                    }

                    Guid? HsID = await healthSystemDb.healthSystemHealthRegions
                        .Where(hr => hr.HealthRegionId == HrID)
                        .Select(hs=>hs.HealthSystemId)
                        .FirstOrDefaultAsync();

                    if (HsID == null)
                    {
                        return false;
                    }
                }

                //check whether the organization claims belong to the same region or not, even if the health system is same
                var orgIds=registerRequestDTO.ClaimIds;
                var healthRegions =healthSystemDb.healthRegionOrganizations
                .Where(hro => orgIds.Contains(hro.OrganizationId))
                .Select(hro => hro.HealthRegionId)
                .Distinct()
                .ToList();
                if (healthRegions.Count != 1)
                {
                    Console.WriteLine("All organizations ids arent belonging to the same health region.");
                    return false;
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
                     Guid? HrID = await healthSystemDb.healthRegionOrganizations
                     .Where(org => org.OrganizationId == claimId)
                     .Select(org => org.HealthRegionId)
                     .FirstOrDefaultAsync();

                    if (HrID == null)
                    {
                        return false;
                    }
                    else if (HrID.ToString() != claimedId)
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
