using System.Threading;
using System.Threading.Tasks;
using MediatR;
using EventBooking.Application.Interfaces;

namespace EventBooking.Application.Features.Customers.Commands
{
    /// <summary>
    /// Handler for deleting a customer
    /// </summary>
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
    {
        private readonly ICustomerRepository _repo;

        public DeleteCustomerCommandHandler(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            await _repo.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
