using _20263recetario.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//cerebro del orm que pasamos y que no del modelo al dto
namespace _20263recetario.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options ) : base( options ) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);//heredamos lo de identity

            //----------------------Category-pasandoles las propiedades alos atributos de las tablas
            builder.Entity<Category>(e =>
            {
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Name).HasMaxLength(60).IsRequired();
                e.Property(x => x.Description).HasMaxLength(250);
                e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.HasIndex(x => x.Name).IsUnique();
            });
        }
        public DbSet<Category> Categories => Set<Category>();//modelo(singu) a orm(dtos)pasamos el nombre del modelo original para copearlo le cambiamos el nombre para poder 
    }
}
