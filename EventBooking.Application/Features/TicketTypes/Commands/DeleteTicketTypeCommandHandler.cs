using System.Threading;
using System.Threading.Tasks;
using MediatR;
using EventBooking.Application.Interfaces;

namespace EventBooking.Application.Features.TicketTypes.Commands
{
    public class DeleteTicketTypeCommandHandler : IRequestHandler<DeleteTicketTypeCommand, Unit>
    {
        private readonly ITicketTypeRepository _repo;

        public DeleteTicketTypeCommandHandler(ITicketTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteTicketTypeCommand request, CancellationToken cancellationToken)
        {
            await _repo.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
