using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;

namespace EventBooking.Application.Features.Bookings.Queries
{
    /// <summary>
    /// Handler for retrieving a specific booking by ID
    /// </summary>
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDto>
    {
        private readonly IBookingRepository _repo;
        private readonly IMapper _mapper;

        public GetBookingByIdQueryHandler(IBookingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _repo.GetByIdAsync(request.Id);
            return booking == null ? null! : _mapper.Map<BookingDto>(booking);
        }
    }
}
