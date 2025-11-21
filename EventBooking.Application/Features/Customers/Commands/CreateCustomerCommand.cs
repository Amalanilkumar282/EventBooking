using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Customers.Commands
{
    /// <summary>
    /// Command for creating a new customer
    /// </summary>
    public class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public CreateCustomerDto Create { get; set; } = null!;
    }
}
