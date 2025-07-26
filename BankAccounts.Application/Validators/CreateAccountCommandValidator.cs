using BankAccounts.Application.Commands;
using BankAccounts.Application.Interfaces;
using BankAccounts.Domain.Enums;
using FluentValidation;

namespace BankAccounts.Application.Validators;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator(
        IOwnerVerificationService ownerVerificationService, 
        ICurrencyService currencyService)
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Owner ID is required")
            .MustAsync(async (id, ct) => await ownerVerificationService.OwnerExistsAsync(id, ct))
            .Must(id => id != Guid.Empty).WithMessage("Invalid Owner ID");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid account type");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Must(currencyService.IsSupportedCurrency)
            .Length(3).WithMessage("Currency code must be 3 characters")
            .Matches("^[A-Z]{3}$").WithMessage("Invalid currency format (ISO 4217)");

        RuleFor(x => x.InterestRate)
            .Must((cmd, rate) => 
                cmd.Type != AccountType.Checking || rate == null)
            .WithMessage("Interest rate must be null for checking accounts")
            .GreaterThanOrEqualTo(0).When(cmd => cmd.Type != AccountType.Checking)
            .WithMessage("Interest rate cannot be negative");
    }
}