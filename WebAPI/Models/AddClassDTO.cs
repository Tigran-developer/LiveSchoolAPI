using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class AddClassDTO
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [Range(15, 480)]
        public int DurationInMinutes { get; set; }

        public bool IsRecurring { get; set; } = false;

        public string? RecurrencePattern { get; set; }

        [MaxLength(500)]
        public string? ZoomLink { get; set; }

        [Required]
        [Range(1, 100)]
        public int MaxParticipants { get; set; }

        [Required]
        public Guid TeacherId { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public int? NotifyBeforeMinutes { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 4)]
        public int DifficultyId { get; set; }

        public string[]? Tags { get; set; }

        public string[]? Materials { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        public bool IsOnline { get; set; } = true;
    }
}
