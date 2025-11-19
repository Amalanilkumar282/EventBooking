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
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDto>
    {
        private readonly IBookingRepository _repo;
        private readonly IEventRepository _eventRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly ITicketTypeRepository _ticketTypeRepo;
        private readonly IMapper _mapper;

        public CreateBookingCommandHandler(
            IBookingRepository repo, 
            IEventRepository eventRepo, 
            ICustomerRepository customerRepo,
            ITicketTypeRepository ticketTypeRepo,
            IMapper mapper)
        {
            _repo = repo;
            _eventRepo = eventRepo;
            _customerRepo = customerRepo;
            _ticketTypeRepo = ticketTypeRepo;
            _mapper = mapper;
        }

        public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // Validate that the event exists
            if (!await _eventRepo.ExistsAsync(request.Create.EventId))
            {
                throw new InvalidOperationException($"Event with ID '{request.Create.EventId}' does not exist.");
            }

            // Validate that the customer exists
            if (!await _customerRepo.ExistsAsync(request.Create.CustomerId))
            {
                throw new InvalidOperationException($"Customer with ID '{request.Create.CustomerId}' does not exist.");
            }

            // Validate seats is greater than zero
            if (request.Create.Seats <= 0)
            {
                throw new InvalidOperationException("Seats must be greater than zero.");
            }

            var booking = _mapper.Map<Booking>(request.Create);
            booking.Id = Guid.NewGuid();
            booking.CreatedAt = DateTime.UtcNow;
            booking.Status = BookingStatus.Pending;

            // Calculate total price
            if (request.Create.TicketTypeId.HasValue)
            {
                // Validate ticket type exists
                var ticketType = await _ticketTypeRepo.GetByIdAsync(request.Create.TicketTypeId.Value);
                if (ticketType == null)
                {
                    throw new InvalidOperationException($"TicketType with ID '{request.Create.TicketTypeId.Value}' does not exist.");
                }

                // Validate ticket type belongs to the event
                if (ticketType.EventId != request.Create.EventId)
                {
                    throw new InvalidOperationException($"TicketType does not belong to the specified event.");
                }

                booking.TotalPrice = ticketType.Price * request.Create.Seats;
            }
            else
            {
                // If no ticket type specified, price is 0 (could be free event or handle differently)
                booking.TotalPrice = 0;
            }

            await _repo.AddAsync(booking);
            return _mapper.Map<BookingDto>(booking);
        }
    }
}
