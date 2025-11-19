using System.Collections.Generic;
using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.TicketTypes.Queries
{
    public class GetTicketTypesQuery : IRequest<List<TicketTypeDto>>
    {
    }
}
