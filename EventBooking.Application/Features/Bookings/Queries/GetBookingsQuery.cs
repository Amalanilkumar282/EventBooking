using System.Collections.Generic;
using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Bookings.Queries
{
    /// <summary>
    /// Query for retrieving all bookings
    /// </summary>
    public class GetBookingsQuery : IRequest<List<BookingDto>>
    {
    }
}
