using MediatR;
using EventBooking.Application.DTOs;
using System;

namespace EventBooking.Application.Features.Customers.Commands
{
    /// <summary>
    /// Command for updating an existing customer
    /// </summary>
    public class UpdateCustomerCommand : IRequest<CustomerDto?>
    {
        public Guid Id { get; set; }
        public UpdateCustomerDto Update { get; set; } = null!;
    }
}
