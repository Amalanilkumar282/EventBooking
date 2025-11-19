using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;

namespace EventBooking.Application.Features.Customers.Commands
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto?>
    {
        private readonly ICustomerRepository _repo;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(ICustomerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CustomerDto?> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetByIdAsync(request.Id);
            if (existing == null) return null;

            // Check if email is being updated and if it already exists
            if (!string.IsNullOrEmpty(request.Update.Email) && request.Update.Email != existing.Email)
            {
                if (await _repo.EmailExistsAsync(request.Update.Email, request.Id))
                {
                    throw new InvalidOperationException($"Customer with email '{request.Update.Email}' already exists.");
                }
            }

            _mapper.Map(request.Update, existing);
            await _repo.UpdateAsync(existing);

            return _mapper.Map<CustomerDto>(existing);
        }
    }
}
