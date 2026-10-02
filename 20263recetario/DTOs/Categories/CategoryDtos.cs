using System.ComponentModel.DataAnnotations;

namespace _20263recetario.DTOs.Categories
{
    public class CategoryDtos
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }

    public class CategoryCreateDtos
    {
        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; } = default!;
        [StringLength(250)]
        public string? Description { get; set; }
    }

    public class CategoryUpdateDtos
    {
        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; } = default!;
        [StringLength(250)]
        public string? Description { get; set; }
    }
}
