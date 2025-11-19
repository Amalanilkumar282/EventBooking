using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using EventBooking.Application.Features.Customers.Commands;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Application.DTOs;

namespace EventBooking.Test.Handlers
{
    public class UpdateCustomerCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_ReturnNull_When_NotFound()
        {
            var mockRepo = new Mock<ICustomerRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Customer?)null);

            var handler = new UpdateCustomerCommandHandler(mockRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new UpdateCustomerCommand { Id = Guid.NewGuid(), Update = new UpdateCustomerDto { FirstName = "X" } }, CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Throw_When_EmailUpdatedAndExists()
        {
            var existing = new Customer { Id = Guid.NewGuid(), Email = "old@example.com" };

            var mockRepo = new Mock<ICustomerRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.GetByIdAsync(existing.Id)).ReturnsAsync(existing);
            mockRepo.Setup(r => r.EmailExistsAsync("new@example.com", existing.Id)).ReturnsAsync(true);

            var handler = new UpdateCustomerCommandHandler(mockRepo.Object, mockMapper.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new UpdateCustomerCommand { Id = existing.Id, Update = new UpdateCustomerDto { Email = "new@example.com" } }, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Update_When_Valid()
        {
            var existing = new Customer { Id = Guid.NewGuid(), FirstName = "Old", LastName = "Name", Email = "old@example.com" };

            var mockRepo = new Mock<ICustomerRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.GetByIdAsync(existing.Id)).ReturnsAsync(existing);
            mockRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), existing.Id)).ReturnsAsync(false);
            mockRepo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask).Verifiable();

            mockMapper.Setup(m => m.Map(It.IsAny<UpdateCustomerDto>(), existing)).Callback<UpdateCustomerDto, Customer>((u, c) => { if (!string.IsNullOrEmpty(u.FirstName)) c.FirstName = u.FirstName; if (!string.IsNullOrEmpty(u.LastName)) c.LastName = u.LastName; if (!string.IsNullOrEmpty(u.Email)) c.Email = u.Email; });
            mockMapper.Setup(m => m.Map<CustomerDto>(existing)).Returns(new CustomerDto { Id = existing.Id, FirstName = existing.FirstName, LastName = existing.LastName, Email = existing.Email });

            var handler = new UpdateCustomerCommandHandler(mockRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new UpdateCustomerCommand { Id = existing.Id, Update = new UpdateCustomerDto { FirstName = "New", Email = "new@example.com" } }, CancellationToken.None);

            result.Should().NotBeNull();
            result.FirstName.Should().Be("New");
            mockRepo.Verify(r => r.UpdateAsync(existing), Times.Once);
        }
    }
}
