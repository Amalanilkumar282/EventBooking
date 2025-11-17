using FluentValidation;
using EventBooking.Application.Features.Events.Commands;

namespace EventBooking.Application.Validators
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(x => x.Create).NotNull().WithMessage("Create payload is required");
            RuleFor(x => x.Create.Name).NotEmpty().WithMessage("Event name is required").MaximumLength(200);
            RuleFor(x => x.Create.Venue).MaximumLength(200);
            RuleFor(x => x.Create.Description).MaximumLength(1000);
            RuleFor(x => x.Create.Capacity).GreaterThan(0).WithMessage("Capacity must be greater than zero");
            RuleFor(x => x.Create.StartDate).NotEmpty().WithMessage("StartDate is required");
            // EndDate, if provided, must be after StartDate
            RuleFor(x => x.Create.EndDate)
                .GreaterThan(x => x.Create.StartDate)
                .When(x => x.Create.EndDate.HasValue)
                .WithMessage("EndDate must be after StartDate");
        }
    }
}
