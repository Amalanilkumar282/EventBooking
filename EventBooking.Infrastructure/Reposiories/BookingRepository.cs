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
    public class BookingRepository : IBookingRepository
    {
        private readonly EventBookingDbContext _db;

        public BookingRepository(EventBookingDbContext db)
        {
            _db = db;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _db.Bookings.ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _db.Bookings.FindAsync(id);
        }

        public async Task<List<Booking>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _db.Bookings
                .Where(b => b.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetByEventIdAsync(Guid eventId)
        {
            return await _db.Bookings
                .Where(b => b.EventId == eventId)
                .ToListAsync();
        }

        public async Task AddAsync(Booking booking)
        {
            await _db.Bookings.AddAsync(booking);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Booking booking)
        {
            _db.Bookings.Update(booking);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null) return;
            _db.Bookings.Remove(booking);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _db.Bookings.AnyAsync(b => b.Id == id);
        }
    }
}
