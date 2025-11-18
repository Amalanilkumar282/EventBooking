using MediatR;
using EventBooking.Application.DTOs;
using System;

namespace EventBooking.Application.Features.Events.Queries
{
    public class GetEventByIdQuery : IRequest<EventDto>
    {
        public Guid Id { get; set; }
    }
}
