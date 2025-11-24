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
    /// <summary>
    /// API controller for managing events
    /// </summary>
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

        /// <summary>
        /// Get events with optional search and paging
        /// </summary>
        // GET: api/events?search=term&page=1&pageSize=20
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var q = new GetEventsQuery { Search = search, Page = page, PageSize = pageSize };
            var events = await _mediator.Send(q);
            return Ok(events);
        }

        /// <summary>
        /// Get a specific event by ID
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <returns>Event details or 404 if not found</returns>
        // GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var ev = await _mediator.Send(new GetEventByIdQuery { Id = id });
            if (ev == null) return NotFound();
            return Ok(ev);
        }

        /// <summary>
        /// Create a new event
        /// </summary>
        /// <param name="create">Event creation data</param>
        /// <returns>Created event</returns>
        // POST: api/events
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEventDto create)
        {
            var cmd = new CreateEventCommand { Create = create };
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing event
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <param name="update">Event update data</param>
        /// <returns>Updated event or 404 if not found</returns>
        // PUT: api/events/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateEventDto update)
        {
            var cmd = new UpdateEventCommand { Id = id, Update = update };
            var result = await _mediator.Send(cmd);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Delete an event
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <returns>No content</returns>
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
