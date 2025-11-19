using System.Collections.Generic;
using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Customers.Queries
{
    public class GetCustomersQuery : IRequest<List<CustomerDto>>
    {
    }
}
