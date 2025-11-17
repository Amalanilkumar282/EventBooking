using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Events.Commands
{
    public class CreateEventCommand : IRequest<EventDto>
    {
        public CreateEventDto Create { get; set; } = null!;
    }
}
