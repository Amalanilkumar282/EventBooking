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

        public async Task<List<Booking>> GetAllAsync()
        {
            _logger.LogDebug("Fetching all bookings");
            return await _db.Bookings.ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            _logger.LogDebug("Fetching booking by Id={BookingId}", id);
            return await _db.Bookings.FindAsync(id);
        }

        public async Task<List<Booking>> GetByCustomerIdAsync(Guid customerId)
        {
            _logger.LogDebug("Fetching bookings for CustomerId={CustomerId}", customerId);
            return await _db.Bookings
                .Where(b => b.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetByEventIdAsync(Guid eventId)
        {
            _logger.LogDebug("Fetching bookings for EventId={EventId}", eventId);
            return await _db.Bookings
                .Where(b => b.EventId == eventId)
                .ToListAsync();
        }

        public async Task AddAsync(Booking booking)
        {
            _logger.LogDebug("Adding booking EventId={EventId} CustomerId={CustomerId} Seats={Seats}", booking.EventId, booking.CustomerId, booking.Seats);
            await _db.Bookings.AddAsync(booking);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Booking added BookingId={BookingId} EventId={EventId} CustomerId={CustomerId}", booking.Id, booking.EventId, booking.CustomerId);
        }

        public async Task UpdateAsync(Booking booking)
        {
            _logger.LogDebug("Updating booking BookingId={BookingId}", booking.Id);
            _db.Bookings.Update(booking);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Booking updated BookingId={BookingId}", booking.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            _logger.LogDebug("Deleting booking BookingId={BookingId}", id);
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null) return;
            _db.Bookings.Remove(booking);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Booking deleted BookingId={BookingId}", id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            _logger.LogDebug("Checking existence for BookingId={BookingId}", id);
            return await _db.Bookings.AnyAsync(b => b.Id == id);
        }

        public IQueryable<Booking> GetQueryable()
        {
            return _db.Bookings.AsQueryable();
        }

        public async Task<List<Booking>> GetPagedAsync(int page, int pageSize)
        {
            var ps = pageSize <= 0 ? 20 : (pageSize > 100 ? 100 : pageSize);
            var p = page <= 0 ? 1 : page;

            return await _db.Bookings
                .OrderBy(b => b.CreatedAt)
                .Skip((p - 1) * ps)
                .Take(ps)
                .ToListAsync();
        }
    }
}
