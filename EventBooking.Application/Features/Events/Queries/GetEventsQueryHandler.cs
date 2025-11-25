using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;

namespace EventBooking.Application.Features.Events.Queries
{
    /// <summary>
    /// Handler for retrieving events with filtering and paging
    /// </summary>
    public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, List<EventDto>>
    {
        private readonly IEventRepository _repo;
        private readonly IMapper _mapper;

        public GetEventsQueryHandler(IEventRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await _repo.GetPagedAsync(request.Search, request.Page, request.PageSize);
            return _mapper.Map<List<EventDto>>(events);
        }
    }
}
