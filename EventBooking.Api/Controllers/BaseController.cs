using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;

namespace EventBooking.Api.Controllers
{
    /// <summary>
    /// Common base controller to hold shared dependencies for API controllers
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator? _mediator;
        protected readonly IMapper? _mapper;

        protected BaseController()
        {
            _mediator = null;
            _mapper = null;
        }

        protected BaseController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
    }
}
