using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities
{
    public class Class
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool IsRecurring { get; set; }

        [MaxLength(100)]
        public string? RecurrencePattern { get; set; }

        [Url]
        [MaxLength(500)]
        public string? ZoomLink { get; set; }

        public int? MaxParticipants { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; }

        public int NotifyBeforeMinutes { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public Guid TeacherId { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public Teacher? Teacher { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public Admin? Admin { get; set; }

        public ICollection<ClassPupil> ClassPupils { get; set; } = new List<ClassPupil>();
    }

}
