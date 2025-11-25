using MediatR;
using System;

namespace EventBooking.Application.Features.Bookings.Commands
{
    /// <summary>
    /// Command for deleting a booking
    /// </summary>
    public class DeleteBookingCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
