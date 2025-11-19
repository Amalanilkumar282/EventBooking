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
    public class CreateCustomerCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_CreateCustomer_When_EmailNotExists()
        {
            var dto = new CreateCustomerDto { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" };

            var mockRepo = new Mock<ICustomerRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), It.IsAny<Guid?>())).ReturnsAsync(false);

            mockMapper.Setup(m => m.Map<Customer>(dto)).Returns(new Customer { FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email });
            mockMapper.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns((Customer c) => new CustomerDto { Id = c.Id, FirstName = c.FirstName, LastName = c.LastName, Email = c.Email });

            mockRepo.Setup(r => r.AddAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask).Verifiable();

            var handler = new CreateCustomerCommandHandler(mockRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new CreateCustomerCommand { Create = dto }, CancellationToken.None);

            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_EmailExists()
        {
            var dto = new CreateCustomerDto { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" };

            var mockRepo = new Mock<ICustomerRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), It.IsAny<Guid?>())).ReturnsAsync(true);

            var handler = new CreateCustomerCommandHandler(mockRepo.Object, mockMapper.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new CreateCustomerCommand { Create = dto }, CancellationToken.None));
        }
    }
}
