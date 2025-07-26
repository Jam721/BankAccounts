using BankAccounts.Application.Commands;
using BankAccounts.Application.Interfaces;
using FluentValidation;

namespace BankAccounts.Application.Validators;


public class DepositCommandValidator : AbstractValidator<DepositCommand>
{
    public DepositCommandValidator(ICurrencyService currencyService)
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account ID is required")
            .Must(id => id != Guid.Empty).WithMessage("Invalid Account ID");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Must(currencyService.IsSupportedCurrency)
            .Length(3).WithMessage("Currency code must be 3 characters")
            .Matches("^[A-Z]{3}$").WithMessage("Invalid currency format (ISO 4217)");
    }
}