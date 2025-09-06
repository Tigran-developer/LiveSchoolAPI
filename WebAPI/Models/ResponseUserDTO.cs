namespace WebAPI.Models
{
    public class ResponseUserDTO
    {
        public string? Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
