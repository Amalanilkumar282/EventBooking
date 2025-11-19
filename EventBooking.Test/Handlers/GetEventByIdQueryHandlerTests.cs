using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using EventBooking.Application.Features.Events.Queries;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Application.DTOs;

namespace EventBooking.Test.Handlers
{
    public class GetEventByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_ReturnNull_When_NotFound()
        {
            var mockRepo = new Mock<IEventRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Event?)null);

            var handler = new GetEventByIdQueryHandler(mockRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new GetEventByIdQuery { Id = Guid.NewGuid() }, CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Return_Mapped_Event()
        {
            var ev = new Event { Id = Guid.NewGuid(), Name = "E1", Capacity = 10, StartDate = DateTime.UtcNow };
            var mockRepo = new Mock<IEventRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.GetByIdAsync(ev.Id)).ReturnsAsync(ev);
            mockMapper.Setup(m => m.Map<EventDto>(ev)).Returns(new EventDto { Id = ev.Id, Name = ev.Name, Capacity = ev.Capacity, StartDate = ev.StartDate, IsActive = ev.IsActive, CreatedAt = ev.CreatedAt });

            var handler = new GetEventByIdQueryHandler(mockRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new GetEventByIdQuery { Id = ev.Id }, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(ev.Id);
        }
    }
}
