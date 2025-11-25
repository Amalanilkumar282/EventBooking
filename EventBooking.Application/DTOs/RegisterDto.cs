using System;

namespace EventBooking.Application.DTOs
{
    /// <summary>
    /// DTO for registering a new customer with password
    /// </summary>
    public class RegisterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        
        /// <summary>
        /// Plain text password (will be hashed before storing)
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
