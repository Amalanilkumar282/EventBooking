using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace EventBooking.Infrastructure.Reposiories
{
    /// <summary>
    /// Entity Framework implementation of ITicketTypeRepository
    /// </summary>
    public class TicketTypeRepository : ITicketTypeRepository
    {
        private readonly EventBookingDbContext _db;
        private readonly ILogger<TicketTypeRepository> _logger;

        public TicketTypeRepository(EventBookingDbContext db, ILogger<TicketTypeRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<TicketType>> GetAllAsync()
        {
            _logger.LogDebug("Fetching all ticket types");
            return await _db.TicketTypes.ToListAsync();
        }

        public async Task<TicketType?> GetByIdAsync(Guid id)
        {
            _logger.LogDebug("Fetching ticket type by Id={TicketTypeId}", id);
            return await _db.TicketTypes.FindAsync(id);
        }

        public async Task<List<TicketType>> GetByEventIdAsync(Guid eventId)
        {
            _logger.LogDebug("Fetching ticket types for EventId={EventId}", eventId);
            return await _db.TicketTypes
                .Where(t => t.EventId == eventId)
                .ToListAsync();
        }

        public async Task AddAsync(TicketType ticketType)
        {
            _logger.LogDebug("Adding ticket type Name={Name} EventId={EventId}", ticketType.Name, ticketType.EventId);
            await _db.TicketTypes.AddAsync(ticketType);
            await _db.SaveChangesAsync();
            _logger.LogInformation("TicketType added TicketTypeId={TicketTypeId} Name={Name}", ticketType.Id, ticketType.Name);
        }

        public async Task UpdateAsync(TicketType ticketType)
        {
            _logger.LogDebug("Updating ticket type TicketTypeId={TicketTypeId}", ticketType.Id);
            _db.TicketTypes.Update(ticketType);
            await _db.SaveChangesAsync();
            _logger.LogInformation("TicketType updated TicketTypeId={TicketTypeId}", ticketType.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            _logger.LogDebug("Deleting ticket type TicketTypeId={TicketTypeId}", id);
            var ticketType = await _db.TicketTypes.FindAsync(id);
            if (ticketType == null) return;
            _db.TicketTypes.Remove(ticketType);
            await _db.SaveChangesAsync();
            _logger.LogInformation("TicketType deleted TicketTypeId={TicketTypeId}", id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            _logger.LogDebug("Checking existence for TicketTypeId={TicketTypeId}", id);
            return await _db.TicketTypes.AnyAsync(t => t.Id == id);
        }
    }
}
