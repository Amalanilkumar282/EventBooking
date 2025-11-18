using System;

namespace EventBooking.Application.DTOs
{
    public class UpdateEventDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Venue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Capacity { get; set; }
        public bool? IsActive { get; set; }
    }
}
