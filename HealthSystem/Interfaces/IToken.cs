using HealthSystemApp.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace HealthSystemApp.Interfaces
{
    public interface IToken
    {
        string CreateJWTToken(ApplicationUser user, List<string> roles,List<Guid> claimedIds);
    }
}
