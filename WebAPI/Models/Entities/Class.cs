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
        public int durationInMinutes { get; set; }

        public bool IsRecurring { get; set; }

        [MaxLength(100)]
        public string? RecurrencePattern { get; set; }

        [Url]
        [MaxLength(500)]
        public string? ZoomLink { get; set; }

        public int? MaxParticipants { get; set; }

        public int? ParticipantsCount { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int NotifyBeforeMinutes { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(Status))]
        public Guid StatusId { get; set; }

        public LessonStatus Status { get; set; } = default!;

        [Required]
        public Guid TeacherId { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public Teacher? Teacher { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public Admin? Admin { get; set; }

        public ICollection<ClassPupil> ClassPupils { get; set; } = new List<ClassPupil>();
    }

}
