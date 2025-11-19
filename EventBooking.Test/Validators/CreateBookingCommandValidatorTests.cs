using System;
using Xunit;
using FluentAssertions;
using EventBooking.Application.Features.Bookings.Commands;
using EventBooking.Application.DTOs;
using EventBooking.Application.Validators;

namespace EventBooking.Test.Validators
{
    public class CreateBookingCommandValidatorTests
    {
        private readonly CreateBookingCommandValidator _validator = new CreateBookingCommandValidator();

        [Fact]
        public void Validate_Should_HaveError_When_CreateIsNull()
        {
            var cmd = new CreateBookingCommand { Create = null! };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Create");
        }

        [Fact]
        public void Validate_Should_HaveError_When_EventOrCustomerEmptyOrSeatsInvalid()
        {
            var dto = new CreateBookingDto { EventId = Guid.Empty, CustomerId = Guid.Empty, Seats = 0 };
            var cmd = new CreateBookingCommand { Create = dto };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName.Contains("EventId") || e.PropertyName.Contains("CustomerId") || e.PropertyName.Contains("Seats"));
        }

        [Fact]
        public void Validate_Should_Pass_When_Valid()
        {
            var dto = new CreateBookingDto { EventId = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Seats = 2 };
            var cmd = new CreateBookingCommand { Create = dto };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeTrue();
        }
    }
}
