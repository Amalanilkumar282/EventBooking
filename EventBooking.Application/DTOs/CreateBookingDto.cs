using System;

namespace EventBooking.Application.DTOs
{
    /// <summary>
    /// DTO for creating a new booking
    /// </summary>
    public class CreateBookingDto
    {
        public Guid EventId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? TicketTypeId { get; set; }
        public int Seats { get; set; }
    }
}
