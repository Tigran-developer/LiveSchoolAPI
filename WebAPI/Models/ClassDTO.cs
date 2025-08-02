namespace WebAPI.Models
{
    public class ClassDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public int durationInMinutes { get; set; }
        public bool IsRecurring { get; set; }
        public string? RecurrencePattern { get; set; }
        public string? ZoomLink { get; set; }
        public int? MaxParticipants { get; set; }
        public int? participantsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public int NotifyBeforeMinutes { get; set; }
        public bool IsDeleted { get; set; }

        public SimplePrivateUserDTO Teacher { get; set; }
        public SimplePrivateUserDTO Admin { get; set; }
    }

}
