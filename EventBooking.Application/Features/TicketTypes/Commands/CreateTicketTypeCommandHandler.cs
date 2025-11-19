using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;

namespace EventBooking.Application.Features.TicketTypes.Commands
{
    public class CreateTicketTypeCommandHandler : IRequestHandler<CreateTicketTypeCommand, TicketTypeDto>
    {
        private readonly ITicketTypeRepository _repo;
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;

        public CreateTicketTypeCommandHandler(ITicketTypeRepository repo, IEventRepository eventRepo, IMapper mapper)
        {
            _repo = repo;
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        public async Task<TicketTypeDto> Handle(CreateTicketTypeCommand request, CancellationToken cancellationToken)
        {
            // Validate that the event exists
            if (!await _eventRepo.ExistsAsync(request.Create.EventId))
            {
                throw new InvalidOperationException($"Event with ID '{request.Create.EventId}' does not exist.");
            }

            var ticketType = _mapper.Map<TicketType>(request.Create);
            ticketType.Id = Guid.NewGuid();
            ticketType.Sold = 0;
            ticketType.IsActive = true;

            await _repo.AddAsync(ticketType);
            return _mapper.Map<TicketTypeDto>(ticketType);
        }
    }
}
