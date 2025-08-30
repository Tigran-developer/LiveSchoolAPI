using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities
{
    public class ClassStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}

