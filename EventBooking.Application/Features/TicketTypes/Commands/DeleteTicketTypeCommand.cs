using MediatR;
using System;

namespace EventBooking.Application.Features.TicketTypes.Commands
{
    /// <summary>
    /// Command for deleting a ticket type
    /// </summary>
    public class DeleteTicketTypeCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
