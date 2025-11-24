using System.Collections.Generic;
using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Bookings.Queries
{
    /// <summary>
    /// Query for retrieving bookings with optional paging
    /// </summary>
    public class GetBookingsQuery : IRequest<List<BookingDto>>
    {
        public int? CustomerIdIgnoredForNow { get; set; } // placeholder if later want customer filter
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
