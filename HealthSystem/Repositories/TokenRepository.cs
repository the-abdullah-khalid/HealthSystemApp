using HealthSystemApp.Data;
using HealthSystemApp.Interfaces;
using HealthSystemApp.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthSystemApp.Repositories
{
    public class TokenRepository : IToken
    {
        private readonly IConfiguration configuration;
        private readonly HealthSystemAuthDbContext healthSystemAuthDb;

        public TokenRepository(IConfiguration configuration,HealthSystemAuthDbContext healthSystemAuthDb)
        {
            this.configuration = configuration;
            this.healthSystemAuthDb = healthSystemAuthDb;
        }
        public string CreateJWTToken(ApplicationUser user, List<string> roles, List<Guid> claimedIds)
        {
            // Create claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Add ClaimedIds to claims
            foreach (var claimedId in claimedIds)
            {
                claims.Add(new Claim("ClaimedId", claimedId.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
