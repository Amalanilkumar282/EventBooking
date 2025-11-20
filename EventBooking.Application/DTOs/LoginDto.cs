using System;

namespace EventBooking.Application.DTOs
{
    /// <summary>
    /// DTO for user login request
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Customer's email address (used as username)
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Customer's password (plain text - will be compared with stored hash)
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
