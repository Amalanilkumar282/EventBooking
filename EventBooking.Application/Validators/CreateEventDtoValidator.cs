using FluentValidation;
using EventBooking.Application.DTOs;

namespace EventBooking.Application.Validators
{
    public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
    {
        public CreateEventDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Event name is required").MaximumLength(200);
            RuleFor(x => x.Venue).MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(1000);
            RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Capacity must be greater than zero");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("StartDate is required");
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("EndDate must be after StartDate");
        }
    }
}
