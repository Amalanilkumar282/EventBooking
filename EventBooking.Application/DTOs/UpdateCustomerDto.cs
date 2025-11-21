using System;

namespace EventBooking.Application.DTOs
{
    /// <summary>
    /// DTO for updating an existing customer with partial update support
    /// </summary>
    public class UpdateCustomerDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
