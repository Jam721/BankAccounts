using BankAccounts.Application.Commands;
using FluentValidation;

namespace BankAccounts.Application.Validators;

public class CloseAccountCommandValidator : AbstractValidator<CloseAccountCommand>
{
    public CloseAccountCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account ID is required")
            .Must(id => id != Guid.Empty).WithMessage("Invalid Account ID");
    }
}