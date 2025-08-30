using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities
{
    public class ClassDifficulty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}

