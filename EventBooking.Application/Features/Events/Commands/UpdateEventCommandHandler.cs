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

            // Preserve original values for nullable source members so we don't overwrite them with defaults
            var originalCapacity = existing.Capacity;
            var originalIsActive = existing.IsActive;

            // map updated fields onto existing entity
            _mapper.Map(request.Update, existing);

            // If Capacity or IsActive were not provided in the update DTO (null), restore original values
            if (request.Update?.Capacity == null)
            {
                existing.Capacity = originalCapacity;
            }

            if (request.Update?.IsActive == null)
            {
                existing.IsActive = originalIsActive;
            }

            await _repo.UpdateAsync(existing);

            return _mapper.Map<EventDto>(existing);
        }
    }
}
