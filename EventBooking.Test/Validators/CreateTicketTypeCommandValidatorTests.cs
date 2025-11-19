using System;
using Xunit;
using FluentAssertions;
using EventBooking.Application.Features.TicketTypes.Commands;
using EventBooking.Application.DTOs;
using EventBooking.Application.Validators;

namespace EventBooking.Test.Validators
{
    public class CreateTicketTypeCommandValidatorTests
    {
        private readonly CreateTicketTypeCommandValidator _validator = new CreateTicketTypeCommandValidator();

        [Fact]
        public void Validate_Should_HaveError_When_CreateIsNull()
        {
            var cmd = new CreateTicketTypeCommand { Create = null! };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Create");
        }

        [Fact]
        public void Validate_Should_HaveError_When_RequiredFieldsMissing()
        {
            var dto = new CreateTicketTypeDto { EventId = Guid.Empty, Name = string.Empty, Price = -1, Quantity = 0 };
            var cmd = new CreateTicketTypeCommand { Create = dto };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName.Contains("EventId") || e.PropertyName.Contains("Name") || e.PropertyName.Contains("Price") || e.PropertyName.Contains("Quantity"));
        }

        [Fact]
        public void Validate_Should_Pass_When_Valid()
        {
            var dto = new CreateTicketTypeDto { EventId = Guid.NewGuid(), Name = "VIP", Price = 100m, Quantity = 50 };
            var cmd = new CreateTicketTypeCommand { Create = dto };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeTrue();
        }
    }
}
