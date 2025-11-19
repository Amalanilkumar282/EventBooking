using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventBooking.Domain.Entities;

namespace EventBooking.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);
    }
}
