using Microsoft.AspNetCore.Identity;

namespace HealthSystemApp.Models.Domain
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public Guid ClaimedId { get; set; }

    }
}
