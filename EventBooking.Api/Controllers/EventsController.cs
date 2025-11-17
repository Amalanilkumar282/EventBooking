using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Infrastructure.Persistence;
using EventBooking.Domain.Entities;

namespace EventBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventBookingDbContext _db;
        private readonly IMapper _mapper;

        public EventsController(EventBookingDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // GET: api/events
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var events = await _db.Events.ToListAsync();
            var dto = _mapper.Map<EventDto[]>(events);
            return Ok(dto);
        }

        // GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound();
            return Ok(_mapper.Map<EventDto>(ev));
        }

        // POST: api/events
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEventDto create)
        {
            var ev = _mapper.Map<Event>(create);
            ev.Id = Guid.NewGuid();
            ev.CreatedAt = DateTime.UtcNow;

            await _db.Events.AddAsync(ev);
            await _db.SaveChangesAsync();

            var dto = _mapper.Map<EventDto>(ev);
            return CreatedAtAction(nameof(Get), new { id = ev.Id }, dto);
        }
    }
}
