using HealthSystemApp.DTOs.AuthenticationDTOs;
using HealthSystemApp.Interfaces;
using HealthSystemApp.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IToken tokenRepository;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager,IToken tokenRepository, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.roleManager = roleManager;
        }


        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        //[SkipAuthorizationMiddlewareAttribute]
        [Authorize(Policy = "AdministratorOnly")]
        public async Task<IActionResult> Register([FromForm] RegisterRequestDTO registerRequestDto)
        {
            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                foreach (var role in registerRequestDto.Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        // Role doesn't exist, return BadRequest
                        return BadRequest($"Role '{role}' is invalid.");
                    }
                }
            }

            var identityUser = new ApplicationUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
                HealthSystemId = registerRequestDto.HealthSystemId,
                HealthRegionId = registerRequestDto.HealthRegionId,
                OrganizationId = registerRequestDto.OrganizationId,
                
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                // Add roles to this User
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User is registered successfully! Please login.");
                    }
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

                    if (roles != null)
                    {
                        // Create Token

                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or password incorrect,Please try Again");
        }
    }
}
