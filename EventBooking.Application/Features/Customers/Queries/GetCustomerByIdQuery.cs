using MediatR;
using EventBooking.Application.DTOs;
using System;

namespace EventBooking.Application.Features.Customers.Queries
{
    /// <summary>
    /// Query for retrieving a specific customer by ID
    /// </summary>
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public Guid Id { get; set; }
    }
}
