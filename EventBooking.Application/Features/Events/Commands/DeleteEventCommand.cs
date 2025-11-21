using MediatR;
using System;

namespace EventBooking.Application.Features.Events.Commands
{
    /// <summary>
    /// Command for deleting an event
    /// </summary>
    public class DeleteEventCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
