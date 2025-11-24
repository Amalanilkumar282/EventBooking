using System.Collections.Generic;
using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Events.Queries
{
    /// <summary>
    /// Query for retrieving events with optional search and paging
    /// </summary>
    public class GetEventsQuery : IRequest<List<EventDto>>
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
