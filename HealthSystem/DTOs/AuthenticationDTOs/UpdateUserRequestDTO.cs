using System.ComponentModel.DataAnnotations;

namespace HealthSystemApp.DTOs.AuthenticationDTOs
{
    public class UpdateUserRequestDTO
    {
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }
        
        public List<Guid> RevokedClaimIds { get; set; } // ID of the Health System or Region Or Organization, admin wants to revoke

        public List<Guid> ClaimIds { get; set; } // ID of the Health System or Region Or Organization, the user will belong to
    }
}
