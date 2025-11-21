using System.Collections.Generic;
using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Customers.Queries
{
    /// <summary>
    /// Query for retrieving all customers
    /// </summary>
    public class GetCustomersQuery : IRequest<List<CustomerDto>>
    {
    }
}
