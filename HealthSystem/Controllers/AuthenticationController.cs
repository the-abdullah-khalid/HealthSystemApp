using HealthSystemApp.Data;
using HealthSystemApp.DTOs.AuthenticationDTOs;
using HealthSystemApp.Interfaces;
using HealthSystemApp.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IToken tokenRepository;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly HealthSystemAuthDbContext healthSystemAuthDb;

        public AuthenticationController(UserManager<ApplicationUser> userManager,IToken tokenRepository, RoleManager<IdentityRole> roleManager,HealthSystemAuthDbContext healthSystemAuthDb)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.roleManager = roleManager;
            this.healthSystemAuthDb = healthSystemAuthDb;
        }


        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        //[SkipAuthorizationMiddlewareAttribute]
        [Authorize(Policy = "AdministratorOrHealthSystemAdminOrHealthRegionAdmin")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDto)
        {
            if (registerRequestDto.Role != null && registerRequestDto.Role.Any())
            {
                if (!await roleManager.RoleExistsAsync(registerRequestDto.Role))
                {
                    // Role doesn't exist, return BadRequest
                    return BadRequest($"Role '{registerRequestDto.Role}' is invalid.");
                }
            }

            var identityUser = new ApplicationUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                bool allAdditionsSuccessful = true;

                //get user id
                var userId = identityUser.Id;
                //get role id
                var role = await healthSystemAuthDb.Roles.FirstOrDefaultAsync(r => r.Name == registerRequestDto.Role);
                var roleId = role.Id;

                // Iterate over each ClaimedId and add a separate entry for each one
                foreach (var claimedId in registerRequestDto.ClaimIds)
                {
                    var userRoleClaim = new ApplicationUserRole
                    {
                        UserId = userId,
                        RoleId = roleId,
                        ClaimedId = claimedId
                    };
                    var roleAdditionResult = await healthSystemAuthDb.UserRoles.AddAsync(userRoleClaim);
                    if (roleAdditionResult.State != EntityState.Added)
                    {
                        allAdditionsSuccessful = false;
                        break;
                    }
                }
                if (allAdditionsSuccessful)
                {
                    await healthSystemAuthDb.SaveChangesAsync();
                    return Ok("User is created and role is assigned successfully.");
                }
            }
            return BadRequest("Something went wrong,Please try Again");
        }


        [HttpPost]
        [Route("Login")]
        //[SkipAuthorizationMiddlewareAttribute]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    // Get Roles for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null && roles.Any())
                    {
                        var roleName = roles.First();
                        var role = await healthSystemAuthDb.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                        var roleId = role.Id;


                        //get claimed id/s
                        var claimedIds = await healthSystemAuthDb.UserRoles
                        .Where(ur => ur.UserId == user.Id)
                        .Select(ur => ur.ClaimedId)
                        .ToListAsync();

                        if (claimedIds != null && claimedIds.Any())
                        {
                            // Create Token
                            var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList(), claimedIds);
                            var response = new LoginResponseDTO
                            {
                                JwtToken = jwtToken
                            };

                            return Ok(response);
                        }
                        else
                        {
                            return BadRequest("Username & password are correct,but there are no claims present");
                        }

                    }
                    else
                    {
                        return BadRequest("Username & password are correct,but there is no role assigned");
                    }
                }
            }

            return BadRequest("Username or password incorrect,Please try Again");
        }
    }
}
