using System.Collections.Generic;
using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.TicketTypes.Queries
{
    /// <summary>
    /// Query for retrieving all ticket types
    /// </summary>
    public class GetTicketTypesQuery : IRequest<List<TicketTypeDto>>
    {
    }
}
