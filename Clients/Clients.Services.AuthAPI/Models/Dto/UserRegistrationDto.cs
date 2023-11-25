using System.ComponentModel.DataAnnotations;

namespace Clients.Services.AuthAPI.Models.Dto
{
    public class UserRegistrationDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
