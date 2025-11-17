using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;

namespace EventBooking.Application.Features.Events.Commands
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDto>
    {
        private readonly IEventRepository _repo;
        private readonly IMapper _mapper;

        public CreateEventCommandHandler(IEventRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var ev = _mapper.Map<Event>(request.Create);
            ev.Id = Guid.NewGuid();
            ev.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(ev);
            return _mapper.Map<EventDto>(ev);
        }
    }
}
