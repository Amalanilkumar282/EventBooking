using MediatR;
using System;

namespace EventBooking.Application.Features.Events.Commands
{
    public class DeleteEventCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
