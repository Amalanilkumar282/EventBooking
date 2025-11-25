using System.Threading;
using System.Threading.Tasks;
using MediatR;
using EventBooking.Application.Interfaces;
using System;

namespace EventBooking.Application.Features.Events.Commands
{
    /// <summary>
    /// Handler for deleting an event
    /// </summary>
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, Unit>
    {
        private readonly IEventRepository _repo;

        public DeleteEventCommandHandler(IEventRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            // Make delete idempotent: repository.DeleteAsync already does nothing if entity not found
            await _repo.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
