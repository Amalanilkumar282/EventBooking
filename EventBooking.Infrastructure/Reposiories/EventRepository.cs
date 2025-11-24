using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Infrastructure.Persistence;

namespace EventBooking.Infrastructure.Reposiories
{
    /// <summary>
    /// Entity Framework implementation of IEventRepository
    /// </summary>
    public class EventRepository : IEventRepository
    {
        private readonly EventBookingDbContext _db;

        public EventRepository(EventBookingDbContext db)
        {
            _db = db;
        }

        public async Task<List<Event>> GetAllAsync()
        {
            return await _db.Events.ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(Guid id)
        {
            return await _db.Events.FindAsync(id);
        }

        public async Task AddAsync(Event ev)
        {
            await _db.Events.AddAsync(ev);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Event ev)
        {
            _db.Events.Update(ev);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return;
            _db.Events.Remove(ev);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _db.Events.AnyAsync(e => e.Id == id);
        }
    }
}
