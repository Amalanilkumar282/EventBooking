using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.TicketTypes.Commands
{
    public class CreateTicketTypeCommand : IRequest<TicketTypeDto>
    {
        public CreateTicketTypeDto Create { get; set; } = null!;
    }
}
