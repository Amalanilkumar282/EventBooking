using MediatR;
using EventBooking.Application.DTOs;
using System;

namespace EventBooking.Application.Features.TicketTypes.Commands
{
    /// <summary>
    /// Command for updating an existing ticket type
    /// </summary>
    public class UpdateTicketTypeCommand : IRequest<TicketTypeDto?>
    {
        public Guid Id { get; set; }
        public UpdateTicketTypeDto Update { get; set; } = null!;
    }
}
