using System;
using Xunit;
using FluentAssertions;
using EventBooking.Application.Features.Customers.Commands;
using EventBooking.Application.DTOs;
using EventBooking.Application.Validators;

namespace EventBooking.Test.Validators
{
    public class CreateCustomerCommandValidatorTests
    {
        private readonly CreateCustomerCommandValidator _validator = new CreateCustomerCommandValidator();

        [Fact]
        public void Validate_Should_HaveError_When_CreateIsNull()
        {
            var cmd = new CreateCustomerCommand { Create = null! };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Create");
        }

        [Fact]
        public void Validate_Should_HaveError_When_RequiredFieldsMissingOrInvalid()
        {
            var dto = new CreateCustomerDto { FirstName = string.Empty, LastName = string.Empty, Email = "not-an-email" };
            var cmd = new CreateCustomerCommand { Create = dto };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName.Contains("FirstName") || e.PropertyName.Contains("LastName") || e.PropertyName.Contains("Email"));
        }

        [Fact]
        public void Validate_Should_Pass_When_Valid()
        {
            var dto = new CreateCustomerDto { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var cmd = new CreateCustomerCommand { Create = dto };

            var result = _validator.Validate(cmd);

            result.IsValid.Should().BeTrue();
        }
    }
}
