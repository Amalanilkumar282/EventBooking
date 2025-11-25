using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        // Parameterless (existing) signatures delegating to cancellation-aware overloads
        public Task<List<TicketType>> GetAllAsync() => GetAllAsync(CancellationToken.None);
        public Task<TicketType?> GetByIdAsync(Guid id) => GetByIdAsync(id, CancellationToken.None);
        public Task<List<TicketType>> GetByEventIdAsync(Guid eventId) => GetByEventIdAsync(eventId, CancellationToken.None);
        public Task AddAsync(TicketType ticketType) => AddAsync(ticketType, CancellationToken.None);
        public Task UpdateAsync(TicketType ticketType) => UpdateAsync(ticketType, CancellationToken.None);
        public Task DeleteAsync(Guid id) => DeleteAsync(id, CancellationToken.None);
        public Task<bool> ExistsAsync(Guid id) => ExistsAsync(id, CancellationToken.None);

        public async Task<List<TicketType>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Fetching all ticket types");
            return await _db.TicketTypes.ToListAsync(cancellationToken);
        }

        public async Task<TicketType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Fetching ticket type by Id={TicketTypeId}", id);
            return await _db.TicketTypes.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<List<TicketType>> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Fetching ticket types for EventId={EventId}", eventId);
            return await _db.TicketTypes
                .Where(t => t.EventId == eventId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(TicketType ticketType, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Adding ticket type Name={Name} EventId={EventId}", ticketType.Name, ticketType.EventId);
            await _db.TicketTypes.AddAsync(ticketType, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("TicketType added TicketTypeId={TicketTypeId} Name={Name}", ticketType.Id, ticketType.Name);
        }

        public async Task UpdateAsync(TicketType ticketType, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Updating ticket type TicketTypeId={TicketTypeId}", ticketType.Id);
            _db.TicketTypes.Update(ticketType);
            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("TicketType updated TicketTypeId={TicketTypeId}", ticketType.Id);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Deleting ticket type TicketTypeId={TicketTypeId}", id);
            var ticketType = await _db.TicketTypes.FindAsync(new object[] { id }, cancellationToken);
            if (ticketType == null) return;
            _db.TicketTypes.Remove(ticketType);
            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("TicketType deleted TicketTypeId={TicketTypeId}", id);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Checking existence for TicketTypeId={TicketTypeId}", id);
            return await _db.TicketTypes.AnyAsync(t => t.Id == id, cancellationToken);
        }
    }
}
