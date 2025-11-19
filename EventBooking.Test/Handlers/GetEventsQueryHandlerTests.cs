using System;
using System.Collections.Generic;
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
    public class GetEventsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_Mapped_List()
        {
            var list = new List<Event>
            {
                new Event { Id = Guid.NewGuid(), Name = "E1", Capacity = 10, StartDate = DateTime.UtcNow },
                new Event { Id = Guid.NewGuid(), Name = "E2", Capacity = 20, StartDate = DateTime.UtcNow }
            };

            var mockRepo = new Mock<IEventRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);
            mockMapper.Setup(m => m.Map<List<EventDto>>(list)).Returns(new List<EventDto>
            {
                new EventDto { Id = list[0].Id, Name = list[0].Name, Capacity = list[0].Capacity, StartDate = list[0].StartDate, IsActive = list[0].IsActive, CreatedAt = list[0].CreatedAt },
                new EventDto { Id = list[1].Id, Name = list[1].Name, Capacity = list[1].Capacity, StartDate = list[1].StartDate, IsActive = list[1].IsActive, CreatedAt = list[1].CreatedAt }
            });

            var handler = new GetEventsQueryHandler(mockRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new GetEventsQuery(), CancellationToken.None);

            result.Should().HaveCount(2);
        }
    }
}
