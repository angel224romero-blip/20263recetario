using System.ComponentModel.DataAnnotations;

namespace _20263recetario.Models
{
    public class Category
    {//si usamos orm ocupa id
        public int Id { get; set; }
        [Required]
        [StringLength(250)]//le da prioridad al de dbcontext 
        public string? Name { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;


    }
}
