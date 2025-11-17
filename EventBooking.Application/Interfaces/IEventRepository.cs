using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventBooking.Domain.Entities;

namespace EventBooking.Application.Interfaces
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(Guid id);
        Task AddAsync(Event ev);
    }
}
