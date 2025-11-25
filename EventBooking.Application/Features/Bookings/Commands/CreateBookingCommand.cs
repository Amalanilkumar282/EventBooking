using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Bookings.Commands
{
    /// <summary>
    /// Command for creating a new booking
    /// </summary>
    public class CreateBookingCommand : IRequest<BookingDto>
    {
        public CreateBookingDto Create { get; set; } = null!;
    }
}
