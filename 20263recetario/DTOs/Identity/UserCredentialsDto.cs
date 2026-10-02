using System.ComponentModel.DataAnnotations;

namespace _20263recetario.DTOs.Identity
{//lo que se requiere para logearse
    public class UserCredentialsDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
