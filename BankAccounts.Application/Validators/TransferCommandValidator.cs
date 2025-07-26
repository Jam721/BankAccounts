using BankAccounts.Application.Commands;
using BankAccounts.Application.Interfaces;
using FluentValidation;

namespace BankAccounts.Application.Validators;

public class TransferCommandValidator : AbstractValidator<TransferCommand>
{
    public TransferCommandValidator(ICurrencyService currencyService)
    {
        RuleFor(x => x.FromAccountId)
            .NotEmpty().WithMessage("Source account ID is required")
            .Must(id => id != Guid.Empty).WithMessage("Invalid source Account ID")
            .NotEqual(cmd => cmd.ToAccountId).WithMessage("Source and target accounts must be different");

        RuleFor(x => x.ToAccountId)
            .NotEmpty().WithMessage("Target account ID is required")
            .Must(id => id != Guid.Empty).WithMessage("Invalid target Account ID");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Must(currencyService.IsSupportedCurrency)
            .Length(3).WithMessage("Currency code must be 3 characters")
            .Matches("^[A-Z]{3}$").WithMessage("Invalid currency format (ISO 4217)");
    }
}