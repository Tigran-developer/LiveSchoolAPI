using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Entities;

namespace WebAPI.Models
{
    public class UpdateClassDto
    {
        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Subject { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Range(15, 480)]
        public int? DurationInMinutes { get; set; }

        public bool? IsRecurring { get; set; }

        public string? RecurrencePattern { get; set; }

        [MaxLength(500)]
        public string? ZoomLink { get; set; }

        [Range(1, 100)]
        public int? MaxParticipants { get; set; }

        [Range(0, 1000)]
        public decimal? Price { get; set; }

        [Range(1, 4)]
        public int? DifficultyId { get; set; }

        public string[]? Tags { get; set; }

        public string[]? Materials { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        public bool? IsOnline { get; set; }

        public ClassStatus? Status { get; set; }
    }
}
