using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Features.Events.Queries;
using EventBooking.Application.Features.Events.Commands;

namespace EventBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // GET: api/events
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var events = await _mediator.Send(new GetEventsQuery());
            return Ok(events);
        }

        // GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var ev = await _mediator.Send(new GetEventByIdQuery { Id = id });
            if (ev == null) return NotFound();
            return Ok(ev);
        }

        // POST: api/events
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEventDto create)
        {
            var cmd = new CreateEventCommand { Create = create };
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT: api/events/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateEventDto update)
        {
            var cmd = new UpdateEventCommand { Id = id, Update = update };
            var result = await _mediator.Send(cmd);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // DELETE: api/events/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cmd = new DeleteEventCommand { Id = id };
            await _mediator.Send(cmd);
            return NoContent();
        }
    }
}
