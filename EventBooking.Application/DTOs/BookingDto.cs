using System;

namespace EventBooking.Application.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid? TicketTypeId { get; set; }

        public int Seats { get; set; }

        public decimal TotalPrice { get; set; }

        // Represent status as string in DTO to keep the API layer decoupled from domain enums
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
