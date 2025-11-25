using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;

namespace EventBooking.Application.Features.Customers.Queries
{
    /// <summary>
    /// Handler for retrieving a specific customer by ID
    /// </summary>
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly ICustomerRepository _repo;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(ICustomerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _repo.GetByIdAsync(request.Id);
            return customer == null ? null! : _mapper.Map<CustomerDto>(customer);
        }
    }
}
