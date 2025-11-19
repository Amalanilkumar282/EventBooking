using MediatR;
using System;

namespace EventBooking.Application.Features.Customers.Commands
{
    public class DeleteCustomerCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
