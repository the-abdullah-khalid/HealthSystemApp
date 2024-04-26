using HealthSystemApp.CustomAttributes;
using HealthSystemApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthSystemApp.CustomMiddlewares.Authorization
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration configuration;
   

        public AuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            this.configuration = configuration;
      
        }

        public async Task InvokeAsync(HttpContext context, HealthSystemDbContext healthSystemDb)
        {
            var endpoint = context.GetEndpoint();
            var skip = endpoint?.Metadata.GetMetadata<SkipAuthorizationMiddlewareAttribute>();

            if (skip is not null)
            {
                await _next(context);
                return;
            }

            // Extract JWT token from request headers
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            try
            {
                // Parse JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                // Extract user's roles and entity IDs from claims
                var roles = claimsPrincipal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
                var healthSystemIdClaim = claimsPrincipal.FindFirst("HealthSystemId")?.Value;
                var healthRegionIdClaim = claimsPrincipal.FindFirst("HealthRegionId")?.Value;
                var organizationIdClaim = claimsPrincipal.FindFirst("OrganizationId")?.Value;

                // Perform authorization checks based on user's role and entity IDs

                // For example:
                if (roles.Contains("Administrator"))
                {
                    // Administrator has access to all resources
                    await _next(context);
                    return;
                }
                else if (roles.Contains("HealthSystemAdmin"))
                {
                    var healthSystemIdFromRoute = context.Request.RouteValues["id"]?.ToString()?.ToLower(); 
                    // Check if the user has access to the requested Health System
                    if (IsAuthorizedForHealthSystem(healthSystemIdClaim, healthSystemIdFromRoute))
                    {
                        await _next(context);
                        return;
                    }


                    var healthRegionIdFromRoute = context.Request.RouteValues["id"]?.ToString()?.ToLower();
                    var relation = await healthSystemDb.healthSystemHealthRegions.FirstOrDefaultAsync(r => r.HealthRegionId.ToString() == healthRegionIdFromRoute);

                    var hsID = relation.HealthSystemId;
                    if (hsID.ToString() == healthSystemIdClaim)
                    {
                        await _next(context);
                        return;
                    }
                }
                else if (roles.Contains("RegionAdmin"))
                {
                    var healthRegionIdFromRoute = context.Request.RouteValues["id"]?.ToString()?.ToLower();
                    // Check if the user has access to the requested Health Region
                    if (IsAuthorizedForHealthRegion(healthRegionIdClaim, healthRegionIdFromRoute))
                    {
                        await _next(context);
                        return;
                    }
                }
                else if (roles.Contains("OrganizationAdmin"))
                {
                    var organizationIdFromRoute = context.Request.RouteValues["id"]?.ToString()?.ToLower();
                    // Check if the user has access to the requested Organization
                    if (IsAuthorizedForHealthRegion(organizationIdClaim, organizationIdFromRoute))
                    {
                        await _next(context);
                        return;
                    }
                }


                // If not authorized, return forbidden response
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
            catch (Exception)
            {
                // Token validation failed
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        private bool IsAuthorizedForHealthSystem(string userHealthSystemId, string requestedHealthSystemId)
        {
            return userHealthSystemId == requestedHealthSystemId;
        }
        private bool IsAuthorizedForHealthRegion(string userHealthRegionId, string requestedHealthRegionId)
        {
            return userHealthRegionId == requestedHealthRegionId;
        }
        private bool IsAuthorizedForOrganization(string organizationId, string requestedOrganizationId)
        {
            return organizationId == requestedOrganizationId;
        }

    }

    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }

}
