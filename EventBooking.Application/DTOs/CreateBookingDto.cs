using System;

namespace EventBooking.Application.DTOs
{
    public class CreateBookingDto
    {
        public Guid EventId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? TicketTypeId { get; set; }
        public int Seats { get; set; }
    }
}
