using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Events.Commands
{
    /// <summary>
    /// Command for creating a new event
    /// </summary>
    public class CreateEventCommand : IRequest<EventDto>
    {
        public CreateEventDto Create { get; set; } = null!;
    }
}
