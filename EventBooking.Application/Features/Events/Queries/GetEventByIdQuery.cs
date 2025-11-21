using MediatR;
using EventBooking.Application.DTOs;
using System;

namespace EventBooking.Application.Features.Events.Queries
{
    /// <summary>
    /// Query for retrieving a specific event by ID
    /// </summary>
    public class GetEventByIdQuery : IRequest<EventDto>
    {
        public Guid Id { get; set; }
    }
}
