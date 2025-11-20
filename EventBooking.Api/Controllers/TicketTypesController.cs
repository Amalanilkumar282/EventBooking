using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Features.TicketTypes.Queries;
using EventBooking.Application.Features.TicketTypes.Commands;

namespace EventBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class TicketTypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TicketTypesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // GET: api/tickettypes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ticketTypes = await _mediator.Send(new GetTicketTypesQuery());
            return Ok(ticketTypes);
        }

        // GET: api/tickettypes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var ticketType = await _mediator.Send(new GetTicketTypeByIdQuery { Id = id });
            if (ticketType == null) return NotFound();
            return Ok(ticketType);
        }

        // POST: api/tickettypes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTicketTypeDto create)
        {
            var cmd = new CreateTicketTypeCommand { Create = create };
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT: api/tickettypes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateTicketTypeDto update)
        {
            var cmd = new UpdateTicketTypeCommand { Id = id, Update = update };
            var result = await _mediator.Send(cmd);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // DELETE: api/tickettypes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cmd = new DeleteTicketTypeCommand { Id = id };
            await _mediator.Send(cmd);
            return NoContent();
        }
    }
}
