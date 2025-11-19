using System;

namespace EventBooking.Application.DTOs
{
    public class UpdateTicketTypeDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public bool? IsActive { get; set; }
    }
}
