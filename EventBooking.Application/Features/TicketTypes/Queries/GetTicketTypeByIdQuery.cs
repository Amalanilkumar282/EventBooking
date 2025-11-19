using MediatR;
using EventBooking.Application.DTOs;
using System;

namespace EventBooking.Application.Features.TicketTypes.Queries
{
    public class GetTicketTypeByIdQuery : IRequest<TicketTypeDto>
    {
        public Guid Id { get; set; }
    }
}
