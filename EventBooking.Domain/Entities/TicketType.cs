using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBooking.Domain.Entities
{
    public class TicketType
    {
        public Guid Id { get; set; }

        // FK to Event
        public Guid EventId { get; set; }
        public Event? Event { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // price for this ticket type
        public decimal Price { get; set; }

        // total available quantity for this ticket type
        public int Quantity { get; set; }

        // optional: number already sold
        public int Sold { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
