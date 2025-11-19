using FluentValidation;
using EventBooking.Application.Features.Customers.Commands;

namespace EventBooking.Application.Validators
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Customer ID is required");
            RuleFor(x => x.Update).NotNull().WithMessage("Update payload is required");

            RuleFor(x => x.Update.FirstName)
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters")
                .When(x => x.Update != null && !string.IsNullOrEmpty(x.Update.FirstName));

            RuleFor(x => x.Update.LastName)
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters")
                .When(x => x.Update != null && !string.IsNullOrEmpty(x.Update.LastName));

            RuleFor(x => x.Update.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(256).WithMessage("Email cannot exceed 256 characters")
                .When(x => x.Update != null && !string.IsNullOrEmpty(x.Update.Email));

            RuleFor(x => x.Update.PhoneNumber)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
                .When(x => x.Update != null && !string.IsNullOrEmpty(x.Update.PhoneNumber));
        }
    }
}
