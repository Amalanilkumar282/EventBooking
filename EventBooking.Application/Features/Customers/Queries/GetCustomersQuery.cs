using System.Collections.Generic;
using MediatR;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Features.Customers.Queries
{
    /// <summary>
    /// Query for retrieving customers with optional paging
    /// </summary>
    public class GetCustomersQuery : IRequest<List<CustomerDto>>
    {
        // 1-based page number
        public int Page { get; set; } = 1;
        // page size, clamped by handler
        public int PageSize { get; set; } = 20;
    }
}
