using System;

namespace EventBooking.Application.DTOs
{
    /// <summary>
    /// DTO returned after successful login containing the JWT access token
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// JWT access token - include this in Authorization header as "Bearer {token}"
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Token type - typically "Bearer"
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Token expiration time in seconds (e.g., 3600 = 1 hour)
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Customer information
        /// </summary>
        public CustomerDto? Customer { get; set; }
    }
}
