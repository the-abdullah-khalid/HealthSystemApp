using System.ComponentModel.DataAnnotations;

namespace HealthSystemApp.DTOs.AuthenticationDTOs
{
    public class LoginRequestDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
