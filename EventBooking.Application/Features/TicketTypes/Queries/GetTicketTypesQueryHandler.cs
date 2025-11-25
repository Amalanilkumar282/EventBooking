using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;

namespace EventBooking.Application.Features.TicketTypes.Queries
{
    /// <summary>
    /// Handler for retrieving all ticket types
    /// </summary>
    public class GetTicketTypesQueryHandler : IRequestHandler<GetTicketTypesQuery, List<TicketTypeDto>>
    {
        private readonly ITicketTypeRepository _repo;
        private readonly IMapper _mapper;

        public GetTicketTypesQueryHandler(ITicketTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<TicketTypeDto>> Handle(GetTicketTypesQuery request, CancellationToken cancellationToken)
        {
            var ticketTypes = await _repo.GetAllAsync();
            return _mapper.Map<List<TicketTypeDto>>(ticketTypes);
        }
    }
}
