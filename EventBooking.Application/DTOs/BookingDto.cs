using System;

namespace EventBooking.Application.DTOs
{
    /// <summary>
    /// DTO for booking information returned to clients
    /// </summary>
    public class BookingDto
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid? TicketTypeId { get; set; }

        public int Seats { get; set; }

        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Booking status represented as string (Pending, Confirmed, Cancelled)
        /// </summary>
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
