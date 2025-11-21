using System;

namespace EventBooking.Application.DTOs
{
    /// <summary>
    /// DTO for creating a new ticket type for an event
    /// </summary>
    public class CreateTicketTypeDto
    {
        public Guid EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
