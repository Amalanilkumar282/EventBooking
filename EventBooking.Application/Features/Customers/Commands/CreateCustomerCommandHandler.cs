using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;

namespace EventBooking.Application.Features.Customers.Commands
{
    /// <summary>
    /// Handler for creating a new customer with password hashing
    /// </summary>
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _repo;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(ICustomerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Check if email already exists
            if (await _repo.EmailExistsAsync(request.Create.Email))
            {
                throw new InvalidOperationException($"Customer with email '{request.Create.Email}' already exists.");
            }

            var customer = _mapper.Map<Customer>(request.Create);
            customer.Id = Guid.NewGuid();
            customer.CreatedAt = DateTime.UtcNow;
            
            // Hash the password using BCrypt
            customer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Create.Password);

            await _repo.AddAsync(customer);
            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
