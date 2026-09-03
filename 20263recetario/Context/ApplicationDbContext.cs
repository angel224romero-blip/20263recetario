using _20263recetario.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _20263recetario.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options ) : base( options ) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
                base.OnModelCreating(builder);
        }
    }
}
