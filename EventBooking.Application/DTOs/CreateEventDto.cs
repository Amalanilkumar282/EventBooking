using System;

namespace EventBooking.Application.DTOs
{
    /// <summary>
    /// DTO for creating a new event
    /// </summary>
    public class CreateEventDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Venue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Capacity { get; set; }
    }
}
