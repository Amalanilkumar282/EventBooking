using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;

namespace EventBooking.Application.Features.TicketTypes.Queries
{
    public class GetTicketTypeByIdQueryHandler : IRequestHandler<GetTicketTypeByIdQuery, TicketTypeDto>
    {
        private readonly ITicketTypeRepository _repo;
        private readonly IMapper _mapper;

        public GetTicketTypeByIdQueryHandler(ITicketTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TicketTypeDto> Handle(GetTicketTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var ticketType = await _repo.GetByIdAsync(request.Id);
            return ticketType == null ? null! : _mapper.Map<TicketTypeDto>(ticketType);
        }
    }
}
