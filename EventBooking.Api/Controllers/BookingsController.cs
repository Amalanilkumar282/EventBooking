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
    /// <summary>
    /// API controller for managing bookings
    /// </summary>
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

        /// <summary>
        /// Get bookings with paging
        /// </summary>
        // GET: api/bookings?page=1&pageSize=20
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var q = new GetBookingsQuery { Page = page, PageSize = pageSize };
            var bookings = await _mediator.Send(q);
            return Ok(bookings);
        }

        /// <summary>
        /// Get a specific booking by ID
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>Booking details or 404 if not found</returns>
        // GET: api/bookings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var booking = await _mediator.Send(new GetBookingByIdQuery { Id = id });
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        /// <summary>
        /// Create a new booking
        /// </summary>
        /// <param name="create">Booking creation data</param>
        /// <returns>Created booking</returns>
        // POST: api/bookings
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateBookingDto create)
        {
            var cmd = new CreateBookingCommand { Create = create };
            var result = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <param name="update">Booking update data</param>
        /// <returns>Updated booking or 404 if not found</returns>
        // PUT: api/bookings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateBookingDto update)
        {
            var cmd = new UpdateBookingCommand { Id = id, Update = update };
            var result = await _mediator.Send(cmd);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Delete a booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns>No content</returns>
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
