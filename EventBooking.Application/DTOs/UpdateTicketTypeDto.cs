using System;

namespace EventBooking.Application.DTOs
{
    /// <summary>
    /// DTO for updating an existing ticket type with partial update support
    /// </summary>
    public class UpdateTicketTypeDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public bool? IsActive { get; set; }
    }
}
