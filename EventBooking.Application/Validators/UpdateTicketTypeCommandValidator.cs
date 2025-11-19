using FluentValidation;
using EventBooking.Application.Features.TicketTypes.Commands;

namespace EventBooking.Application.Validators
{
    public class UpdateTicketTypeCommandValidator : AbstractValidator<UpdateTicketTypeCommand>
    {
        public UpdateTicketTypeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("TicketType ID is required");
            RuleFor(x => x.Update).NotNull().WithMessage("Update payload is required");

            RuleFor(x => x.Update.Name)
                .MaximumLength(150).WithMessage("Name cannot exceed 150 characters")
                .When(x => x.Update != null && !string.IsNullOrEmpty(x.Update.Name));

            RuleFor(x => x.Update.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
                .When(x => x.Update != null && !string.IsNullOrEmpty(x.Update.Description));

            RuleFor(x => x.Update.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to zero")
                .When(x => x.Update != null && x.Update.Price.HasValue);

            RuleFor(x => x.Update.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero")
                .When(x => x.Update != null && x.Update.Quantity.HasValue);
        }
    }
}
