using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Features.Customers.Queries;
using EventBooking.Application.Features.Customers.Commands;

namespace EventBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CustomersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _mediator.Send(new GetCustomersQuery());
            return Ok(customers);
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCustomerDto create)
        {
            var cmd = new CreateCustomerCommand { Create = create };
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateCustomerDto update)
        {
            var cmd = new UpdateCustomerCommand { Id = id, Update = update };
            var result = await _mediator.Send(cmd);
            if (result == null) return NotFound();
            return Ok(result);
        }

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
