using MediatR;
using System;

namespace EventBooking.Application.Features.TicketTypes.Commands
{
    public class DeleteTicketTypeCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
