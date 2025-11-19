using FluentValidation;
using EventBooking.Application.Features.Customers.Commands;

namespace EventBooking.Application.Validators
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Create).NotNull().WithMessage("Create payload is required");

            RuleFor(x => x.Create.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters")
                .When(x => x.Create != null);

            RuleFor(x => x.Create.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters")
                .When(x => x.Create != null);

            RuleFor(x => x.Create.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(256).WithMessage("Email cannot exceed 256 characters")
                .When(x => x.Create != null);

            RuleFor(x => x.Create.PhoneNumber)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
                .When(x => x.Create != null && !string.IsNullOrEmpty(x.Create.PhoneNumber));
        }
    }
}
