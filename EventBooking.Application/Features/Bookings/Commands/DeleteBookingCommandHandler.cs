using System.Threading;
using System.Threading.Tasks;
using MediatR;
using EventBooking.Application.Interfaces;

namespace EventBooking.Application.Features.Bookings.Commands
{
    /// <summary>
    /// Handler for deleting a booking
    /// </summary>
    public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand, Unit>
    {
        private readonly IBookingRepository _repo;

        public DeleteBookingCommandHandler(IBookingRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            await _repo.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
