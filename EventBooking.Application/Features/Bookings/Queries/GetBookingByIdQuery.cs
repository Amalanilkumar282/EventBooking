using MediatR;
using EventBooking.Application.DTOs;
using System;

namespace EventBooking.Application.Features.Bookings.Queries
{
    /// <summary>
    /// Query for retrieving a specific booking by ID
    /// </summary>
    public class GetBookingByIdQuery : IRequest<BookingDto>
    {
        public Guid Id { get; set; }
    }
}
