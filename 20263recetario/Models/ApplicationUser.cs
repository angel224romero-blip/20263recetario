using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _20263recetario.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(60)]
        public string ? DisplayName { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
