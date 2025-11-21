using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;

namespace EventBooking.Application.Features.Bookings.Commands
{
    /// <summary>
    /// Handler for updating an existing booking
    /// </summary>
    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, BookingDto?>
    {
        private readonly IBookingRepository _repo;
        private readonly IMapper _mapper;

        public UpdateBookingCommandHandler(IBookingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<BookingDto?> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null) return null;

            var originalSeats = existing.Seats;

            _mapper.Map(request.Update, existing);

            // Restore original values if not provided in update
            if (request.Update?.Seats == null)
            {
                existing.Seats = originalSeats;
            }

            await _repo.UpdateAsync(existing);
            return _mapper.Map<BookingDto>(existing);
        }
    }
}
