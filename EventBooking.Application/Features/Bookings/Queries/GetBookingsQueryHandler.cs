using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;

namespace EventBooking.Application.Features.Bookings.Queries
{
    public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, List<BookingDto>>
    {
        private readonly IBookingRepository _repo;
        private readonly IMapper _mapper;

        public GetBookingsQueryHandler(IBookingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<BookingDto>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _repo.GetAllAsync();
            return _mapper.Map<List<BookingDto>>(bookings);
        }
    }
}
