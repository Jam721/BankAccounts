using BankAccounts.Application.Commands;
using BankAccounts.Domain.Enums;
using FluentValidation;

namespace BankAccounts.Application.Validators;

public class RegisterTransactionCommandValidator : AbstractValidator<RegisterTransactionCommand>
{
    public RegisterTransactionCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account ID is required")
            .Must(id => id != Guid.Empty).WithMessage("Invalid Account ID");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency code must be 3 characters")
            .Matches("^[A-Z]{3}$").WithMessage("Invalid currency format (ISO 4217)");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid transaction type");

        RuleFor(x => x.CounterpartyAccountId)
            .Must((cmd, counterpartyId) => 
                cmd.Type != TransactionType.Credit || counterpartyId.HasValue)
            .WithMessage("Counterparty account is required for transfers")
            .NotEqual(cmd => cmd.AccountId).When(cmd => cmd.CounterpartyAccountId.HasValue)
            .WithMessage("Counterparty cannot be the same as main account");
    }
}