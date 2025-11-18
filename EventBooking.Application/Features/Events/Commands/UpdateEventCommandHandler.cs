using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using System;

namespace EventBooking.Application.Features.Events.Commands
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, EventDto?>
    {
        private readonly IEventRepository _repo;
        private readonly IMapper _mapper;

        public UpdateEventCommandHandler(IEventRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<EventDto?> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null) return null; // return null so controller can return 404

            // map updated fields onto existing entity
            _mapper.Map(request.Update, existing);

            await _repo.UpdateAsync(existing);

            return _mapper.Map<EventDto>(existing);
        }
    }
}
