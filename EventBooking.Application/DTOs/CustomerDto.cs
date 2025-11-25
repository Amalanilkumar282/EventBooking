using System;

namespace EventBooking.Application.DTOs
{
    /// <summary>
    /// DTO for customer information returned to clients (excludes password hash)
    /// </summary>
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
