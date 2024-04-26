using System.ComponentModel.DataAnnotations;

namespace HealthSystemApp.DTOs.AuthenticationDTOs
{
    public class RegisterRequestDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string[] Roles { get; set; }

        public Guid? HealthSystemId { get; set; } // ID of the Health System the user belongs to
        public Guid? HealthRegionId { get; set; } // ID of the Health Region the user belongs to
        public Guid? OrganizationId { get; set; } // ID of the Organization the user belongs to
    }
}
