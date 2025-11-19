using MediatR;
using EventBooking.Application.DTOs;
using System;

namespace EventBooking.Application.Features.Customers.Queries
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public Guid Id { get; set; }
    }
}
