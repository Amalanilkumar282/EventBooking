using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Customers.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public CreateCustomerDto Create { get; set; } = null!;
    }
}
