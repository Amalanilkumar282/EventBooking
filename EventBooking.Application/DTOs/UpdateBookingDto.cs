using System;

namespace EventBooking.Application.DTOs
{
    public class UpdateBookingDto
    {
        public int? Seats { get; set; }
        public string? Status { get; set; }
    }
}
