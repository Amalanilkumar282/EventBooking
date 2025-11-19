using MediatR;
using EventBooking.Application.DTOs;
using System;

namespace EventBooking.Application.Features.TicketTypes.Commands
{
    public class UpdateTicketTypeCommand : IRequest<TicketTypeDto?>
    {
        public Guid Id { get; set; }
        public UpdateTicketTypeDto Update { get; set; } = null!;
    }
}
