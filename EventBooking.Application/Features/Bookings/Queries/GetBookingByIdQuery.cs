using MediatR;
using EventBooking.Application.DTOs;
using System;

namespace EventBooking.Application.Features.Bookings.Queries
{
    public class GetBookingByIdQuery : IRequest<BookingDto>
    {
        public Guid Id { get; set; }
    }
}
