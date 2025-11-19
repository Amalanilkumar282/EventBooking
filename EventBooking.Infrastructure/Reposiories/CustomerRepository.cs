using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Infrastructure.Persistence;

namespace EventBooking.Infrastructure.Reposiories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly EventBookingDbContext _db;

        public CustomerRepository(EventBookingDbContext db)
        {
            _db = db;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _db.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _db.Customers.FindAsync(id);
        }

        public async Task AddAsync(Customer customer)
        {
            await _db.Customers.AddAsync(customer);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return;
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _db.Customers.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return await _db.Customers.AnyAsync(c => c.Email == email && c.Id != excludeId.Value);
            }
            return await _db.Customers.AnyAsync(c => c.Email == email);
        }
    }
}
