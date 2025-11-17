using System.Collections.Generic;
using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Events.Queries
{
    public class GetEventsQuery : IRequest<List<EventDto>>
    {
    }
}
