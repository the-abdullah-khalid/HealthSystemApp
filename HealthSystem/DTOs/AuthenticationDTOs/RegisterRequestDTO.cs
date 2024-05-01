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

        [Required]
        public string Role { get; set; }

        [Required]
        public List<Guid> ClaimIds { get; set; } // ID of the Health System or Region Or Organization, the user will belong to

    }
}
