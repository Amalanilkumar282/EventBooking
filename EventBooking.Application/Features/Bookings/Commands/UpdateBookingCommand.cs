using MediatR;
using EventBooking.Application.DTOs;
using System;

namespace EventBooking.Application.Features.Bookings.Commands
{
    /// <summary>
    /// Command for updating an existing booking
    /// </summary>
    public class UpdateBookingCommand : IRequest<BookingDto?>
    {
        public Guid Id { get; set; }
        public UpdateBookingDto Update { get; set; } = null!;
    }
}
