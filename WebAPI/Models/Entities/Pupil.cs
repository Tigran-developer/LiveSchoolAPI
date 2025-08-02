using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities
{
    public class Pupil
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [EmailAddress]
        [MaxLength(256)]
        public string? Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(450)]
        public string? UserId { get; set; }

        public User? User { get; set; }
    }

}
