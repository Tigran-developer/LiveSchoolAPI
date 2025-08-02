namespace WebAPI.Models
{
    public class AddClassDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid TeacherId { get; set; }
        public DateTime StartTime { get; set;}
        public int durationInMinutes { get; set;}
        public bool IsRecurring { get; set;}
        public string? RecurrencePattern { get; set;}
        public string? ZoomLink { get; set;}
        public int? MaxParticipants { get; set;}
        public Guid CreatedBy { get; set;}
        public int? NotifyBeforeMinutes { get; set;}
    }
}
