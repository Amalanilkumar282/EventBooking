using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Domain.Entities
{
    /// <summary>
    /// Domain entity representing an event that customers can book
    /// </summary>
    public class Event
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Venue { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int Capacity { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
