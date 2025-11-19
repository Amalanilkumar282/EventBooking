using MediatR;
using System;

namespace EventBooking.Application.Features.Bookings.Commands
{
    public class DeleteBookingCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
