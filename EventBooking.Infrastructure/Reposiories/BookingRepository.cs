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
    /// Entity Framework implementation of IBookingRepository
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly EventBookingDbContext _db;
        private readonly ILogger<BookingRepository> _logger;

        public BookingRepository(EventBookingDbContext db, ILogger<BookingRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Parameterless methods for compatibility
        public Task<List<Booking>> GetAllAsync() => GetAllAsync(CancellationToken.None);
        public Task<Booking?> GetByIdAsync(Guid id) => GetByIdAsync(id, CancellationToken.None);
        public Task<List<Booking>> GetByCustomerIdAsync(Guid customerId) => GetByCustomerIdAsync(customerId, CancellationToken.None);
        public Task<List<Booking>> GetByEventIdAsync(Guid eventId) => GetByEventIdAsync(eventId, CancellationToken.None);
        public Task AddAsync(Booking booking) => AddAsync(booking, CancellationToken.None);
        public Task UpdateAsync(Booking booking) => UpdateAsync(booking, CancellationToken.None);
        public Task DeleteAsync(Guid id) => DeleteAsync(id, CancellationToken.None);
        public Task<bool> ExistsAsync(Guid id) => ExistsAsync(id, CancellationToken.None);
        public Task<List<Booking>> GetPagedAsync(int page, int pageSize) => GetPagedAsync(page, pageSize, CancellationToken.None);

        public async Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Fetching all bookings");
            return await _db.Bookings.ToListAsync(cancellationToken);
        }

        public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Fetching booking by Id={BookingId}", id);
            return await _db.Bookings.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<List<Booking>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Fetching bookings for CustomerId={CustomerId}", customerId);
            return await _db.Bookings
                .Where(b => b.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Booking>> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Fetching bookings for EventId={EventId}", eventId);
            return await _db.Bookings
                .Where(b => b.EventId == eventId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Adding booking EventId={EventId} CustomerId={CustomerId} Seats={Seats}", booking.EventId, booking.CustomerId, booking.Seats);
            await _db.Bookings.AddAsync(booking, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Booking added BookingId={BookingId} EventId={EventId} CustomerId={CustomerId}", booking.Id, booking.EventId, booking.CustomerId);
        }

        public async Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Updating booking BookingId={BookingId}", booking.Id);
            _db.Bookings.Update(booking);
            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Booking updated BookingId={BookingId}", booking.Id);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Deleting booking BookingId={BookingId}", id);
            var booking = await _db.Bookings.FindAsync(new object[] { id }, cancellationToken);
            if (booking == null) return;
            _db.Bookings.Remove(booking);
            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Booking deleted BookingId={BookingId}", id);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Checking existence for BookingId={BookingId}", id);
            return await _db.Bookings.AnyAsync(b => b.Id == id, cancellationToken);
        }

        public IQueryable<Booking> GetQueryable()
        {
            return _db.Bookings.AsQueryable();
        }

        public async Task<List<Booking>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var ps = pageSize <= 0 ? 20 : (pageSize > 100 ? 100 : pageSize);
            var p = page <= 0 ? 1 : page;

            return await _db.Bookings
                .OrderBy(b => b.CreatedAt)
                .Skip((p - 1) * ps)
                .Take(ps)
                .ToListAsync(cancellationToken);
        }
    }
}
