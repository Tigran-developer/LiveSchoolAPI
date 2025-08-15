using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities { 
public class LessonStatus
{
    [Key]
    public Guid Id { get; set; } 

    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;
}
}
