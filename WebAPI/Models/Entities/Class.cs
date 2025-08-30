using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities
{
    public class Class
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        [Required, MaxLength(100)]
        public string Subject { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public int DurationInMinutes { get; set; }

        public bool IsRecurring { get; set; }

        public string? RecurrencePattern { get; set; }

        [Url]
        [MaxLength(500)]
        public string? ZoomLink { get; set; }

        [Required]
        [Range(1, 100)]
        public int MaxParticipants { get; set; }

        public int? ParticipantsCount { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int NotifyBeforeMinutes { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey(nameof(Difficulty))]
        public int DifficultyId { get; set; }

        public ClassDifficulty Difficulty { get; set; } = default!;

        public string[]? Tags { get; set; }

        public string[]? Materials { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        public bool IsOnline { get; set; } = true;

        [MaxLength(500)]
        public string? RecordingUrl { get; set; }

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
