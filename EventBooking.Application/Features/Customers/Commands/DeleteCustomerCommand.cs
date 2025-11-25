using MediatR;
using System;

namespace EventBooking.Application.Features.Customers.Commands
{
    /// <summary>
    /// Command for deleting a customer
    /// </summary>
    public class DeleteCustomerCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
