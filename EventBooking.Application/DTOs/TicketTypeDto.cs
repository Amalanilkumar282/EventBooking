using System;

namespace EventBooking.Application.DTOs
{
    /// <summary>
    /// DTO for ticket type information returned to clients
    /// </summary>
    public class TicketTypeDto
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }
        public bool IsActive { get; set; }
    }
}
