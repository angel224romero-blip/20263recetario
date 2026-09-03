using System.ComponentModel.DataAnnotations;

namespace _20263recetario.DTOs.Identity
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
        
        [Required]
        public string Password { get; set; } = default!;

        [Required]
        [StringLength(60)]
        public string DisplayName { get; set; } = default!;
    }
}
