using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Infrastructure.Persistence;

namespace EventBooking.Infrastructure.Reposiories
{
    /// <summary>
    /// Entity Framework implementation of ITicketTypeRepository
    /// </summary>
    public class TicketTypeRepository : ITicketTypeRepository
    {
        private readonly EventBookingDbContext _db;

        public TicketTypeRepository(EventBookingDbContext db)
        {
            _db = db;
        }

        public async Task<List<TicketType>> GetAllAsync()
        {
            return await _db.TicketTypes.ToListAsync();
        }

        public async Task<TicketType?> GetByIdAsync(Guid id)
        {
            return await _db.TicketTypes.FindAsync(id);
        }

        public async Task<List<TicketType>> GetByEventIdAsync(Guid eventId)
        {
            return await _db.TicketTypes
                .Where(t => t.EventId == eventId)
                .ToListAsync();
        }

        public async Task AddAsync(TicketType ticketType)
        {
            await _db.TicketTypes.AddAsync(ticketType);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TicketType ticketType)
        {
            _db.TicketTypes.Update(ticketType);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var ticketType = await _db.TicketTypes.FindAsync(id);
            if (ticketType == null) return;
            _db.TicketTypes.Remove(ticketType);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _db.TicketTypes.AnyAsync(t => t.Id == id);
        }
    }
}
