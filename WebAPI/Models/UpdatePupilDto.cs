﻿namespace WebAPI.Models
{
    public class UpdatePupilDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
    }
}
