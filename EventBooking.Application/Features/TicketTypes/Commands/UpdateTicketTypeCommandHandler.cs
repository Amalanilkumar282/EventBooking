using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;

namespace EventBooking.Application.Features.TicketTypes.Commands
{
    /// <summary>
    /// Handler for updating an existing ticket type
    /// </summary>
    public class UpdateTicketTypeCommandHandler : IRequestHandler<UpdateTicketTypeCommand, TicketTypeDto?>
    {
        private readonly ITicketTypeRepository _repo;
        private readonly IMapper _mapper;

        public UpdateTicketTypeCommandHandler(ITicketTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TicketTypeDto?> Handle(UpdateTicketTypeCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null) return null;

            var originalPrice = existing.Price;
            var originalQuantity = existing.Quantity;
            var originalIsActive = existing.IsActive;

            _mapper.Map(request.Update, existing);

            // Restore original values if not provided in update
            if (request.Update?.Price == null)
            {
                existing.Price = originalPrice;
            }

            if (request.Update?.Quantity == null)
            {
                existing.Quantity = originalQuantity;
            }

            if (request.Update?.IsActive == null)
            {
                existing.IsActive = originalIsActive;
            }

            await _repo.UpdateAsync(existing);
            return _mapper.Map<TicketTypeDto>(existing);
        }
    }
}
