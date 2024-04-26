using Microsoft.AspNetCore.Identity;

namespace HealthSystemApp.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public Guid? HealthSystemId { get; set; } // ID of the Health System the user belongs to
        public Guid? HealthRegionId { get; set; } // ID of the Health Region the user belongs to
        public Guid? OrganizationId { get; set; } // ID of the Organization the user belongs to
    }
}
