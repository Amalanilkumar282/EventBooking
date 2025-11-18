using MediatR;
using EventBooking.Application.DTOs;
using System;

namespace EventBooking.Application.Features.Events.Commands
{
    public class UpdateEventCommand : IRequest<EventBooking.Application.DTOs.EventDto?>
    {
        public Guid Id { get; set; }
        public UpdateEventDto Update { get; set; } = null!;
    }
}
