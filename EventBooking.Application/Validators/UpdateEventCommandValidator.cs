using FluentValidation;
using EventBooking.Application.Features.Events.Commands;

namespace EventBooking.Application.Validators
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Event Id is required");
            RuleFor(x => x.Update).NotNull().WithMessage("Update payload is required");
            RuleFor(x => x.Update.Name).NotEmpty().WithMessage("Event name is required").MaximumLength(200);
            RuleFor(x => x.Update.Venue).MaximumLength(200);
            RuleFor(x => x.Update.Description).MaximumLength(1000);
            RuleFor(x => x.Update.Capacity).GreaterThan(0).WithMessage("Capacity must be greater than zero");
            RuleFor(x => x.Update.EndDate)
                .GreaterThan(x => x.Update.StartDate)
                .When(x => x.Update.EndDate.HasValue)
                .WithMessage("EndDate must be after StartDate");
        }
    }
}
