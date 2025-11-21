using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;

namespace EventBooking.Application.Features.Events.Queries
{
    /// <summary>
    /// Handler for retrieving a specific event by ID
    /// </summary>
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto>
    {
        private readonly IEventRepository _repo;
        private readonly IMapper _mapper;

        public GetEventByIdQueryHandler(IEventRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var ev = await _repo.GetByIdAsync(request.Id);
            return ev == null ? null! : _mapper.Map<EventDto>(ev);
        }
    }
}
