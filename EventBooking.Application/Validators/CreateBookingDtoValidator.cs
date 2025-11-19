using FluentValidation;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Validators
{
    public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
    {
        public CreateBookingDtoValidator()
        {
            RuleFor(x => x.EventId)
                .NotEmpty().WithMessage("Event ID is required");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required");

            RuleFor(x => x.Seats)
                .GreaterThan(0).WithMessage("Seats must be greater than zero");
        }
    }
}
