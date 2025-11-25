using System;

namespace EventBooking.Application.DTOs
{
    /// <summary>
    /// DTO for updating an existing booking with partial update support
    /// </summary>
    public class UpdateBookingDto
    {
        public int? Seats { get; set; }
        public string? Status { get; set; }
    }
}
