using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Domain.Entities
{
    public enum BookingStatus
    {
        Pending = 0,
        Confirmed = 1,
        Cancelled = 2
    }

    public class Booking
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }
        public Event? Event { get; set; }

        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public Guid? TicketTypeId { get; set; }
        public TicketType? TicketType { get; set; }

        public int Seats { get; set; }

        public decimal TotalPrice { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public byte[]? RowVersion { get; set; }
    }
}