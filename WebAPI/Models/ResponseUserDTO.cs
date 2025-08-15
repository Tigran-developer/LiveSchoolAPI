namespace WebAPI.Models
{
    public class ResponseUserDTO
    {
        public string? Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public bool isTeacher { get; set; }
    }
}
