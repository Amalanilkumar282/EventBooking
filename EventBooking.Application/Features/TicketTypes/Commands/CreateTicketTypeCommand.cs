using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.TicketTypes.Commands
{
    /// <summary>
    /// Command for creating a new ticket type
    /// </summary>
    public class CreateTicketTypeCommand : IRequest<TicketTypeDto>
    {
        public CreateTicketTypeDto Create { get; set; } = null!;
    }
}
