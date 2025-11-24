using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Features.Customers.Queries;
using EventBooking.Application.Features.Customers.Commands;

namespace EventBooking.Api.Controllers
{
    /// <summary>
    /// API controller for managing customers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CustomersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get customers with paging
        /// </summary>
        // GET: api/customers?page=1&pageSize=20
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var q = new GetCustomersQuery { Page = page, PageSize = pageSize };
            var customers = await _mediator.Send(q);
            return Ok(customers);
        }

        /// <summary>
        /// Get a specific customer by ID
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Customer details or 404 if not found</returns>
        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <param name="create">Customer creation data</param>
        /// <returns>Created customer</returns>
        // POST: api/customers
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCustomerDto create)
        {
            var cmd = new CreateCustomerCommand { Create = create };
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing customer
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <param name="update">Customer update data</param>
        /// <returns>Updated customer or 404 if not found</returns>
        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateCustomerDto update)
        {
            var cmd = new UpdateCustomerCommand { Id = id, Update = update };
            var result = await _mediator.Send(cmd);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Delete a customer
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>No content</returns>
        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cmd = new DeleteCustomerCommand { Id = id };
            await _mediator.Send(cmd);
            return NoContent();
        }
    }
}
