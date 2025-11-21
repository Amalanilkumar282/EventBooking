using System.Collections.Generic;
using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Events.Queries
{
    /// <summary>
    /// Query for retrieving all events
    /// </summary>
    public class GetEventsQuery : IRequest<List<EventDto>>
    {
    }
}
