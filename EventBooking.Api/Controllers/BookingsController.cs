using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Features.Bookings.Queries;
using EventBooking.Application.Features.Bookings.Commands;

namespace EventBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BookingsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var bookings = await _mediator.Send(new GetBookingsQuery());
            return Ok(bookings);
        }

        // GET: api/bookings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var booking = await _mediator.Send(new GetBookingByIdQuery { Id = id });
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateBookingDto create)
        {
            var cmd = new CreateBookingCommand { Create = create };
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT: api/bookings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateBookingDto update)
        {
            var cmd = new UpdateBookingCommand { Id = id, Update = update };
            var result = await _mediator.Send(cmd);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // DELETE: api/bookings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cmd = new DeleteBookingCommand { Id = id };
            await _mediator.Send(cmd);
            return NoContent();
        }
    }
}
