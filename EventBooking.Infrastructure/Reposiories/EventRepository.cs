using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace EventBooking.Infrastructure.Reposiories
{
    /// <summary>
    /// Entity Framework implementation of IEventRepository
    /// </summary>
    public class EventRepository : IEventRepository
    {
        private readonly EventBookingDbContext _db;
        private readonly ILogger<EventRepository> _logger;

        public EventRepository(EventBookingDbContext db, ILogger<EventRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<Event>> GetAllAsync()
        {
            _logger.LogDebug("Fetching all events");
            return await _db.Events.ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(Guid id)
        {
            _logger.LogDebug("Fetching event by Id={EventId}", id);
            return await _db.Events.FindAsync(id);
        }

        public async Task AddAsync(Event ev)
        {
            _logger.LogDebug("Adding event Name={EventName}", ev.Name);
            await _db.Events.AddAsync(ev);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Event added EventId={EventId} Name={EventName}", ev.Id, ev.Name);
        }

        public async Task UpdateAsync(Event ev)
        {
            _logger.LogDebug("Updating event EventId={EventId}", ev.Id);
            _db.Events.Update(ev);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Event updated EventId={EventId}", ev.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            _logger.LogDebug("Deleting event EventId={EventId}", id);
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return;
            _db.Events.Remove(ev);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Event deleted EventId={EventId}", id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            _logger.LogDebug("Checking existence for EventId={EventId}", id);
            return await _db.Events.AnyAsync(e => e.Id == id);
        }
    }
}
