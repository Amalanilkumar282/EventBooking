using System.Collections.Generic;
using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Bookings.Queries
{
    public class GetBookingsQuery : IRequest<List<BookingDto>>
    {
    }
}
