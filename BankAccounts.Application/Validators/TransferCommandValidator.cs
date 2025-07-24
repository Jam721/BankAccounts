using BankAccounts.Application.Commands;
using BankAccounts.Domain.Enums;
using BankAccounts.Domain.Interfaces;
using FluentValidation;

namespace BankAccounts.Application.Validators;

public class TransferCommandValidator : AbstractValidator<TransferCommand>
{
    private readonly IAccountRepository _accountRepository;

    public TransferCommandValidator(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;

        RuleFor(cmd => cmd)
            .MustAsync(async (cmd, ct) => 
                await HaveSufficientFunds(cmd.FromAccountId, cmd.Amount, ct))
            .WithMessage("Insufficient funds")
            .WithName("Amount");
    }
    
    private async Task<bool> HaveSufficientFunds(Guid accountId, decimal amount, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);
    
        if (account == null)
            throw new KeyNotFoundException($"Account with ID {accountId} not found");

        switch (account.Type)
        {
            case AccountType.Checking:
            case AccountType.Deposit:
                return account.Balance >= amount;
        
            case AccountType.Credit:
                
                var creditLimit = account.InterestRate.HasValue 
                    ? Math.Abs(account.InterestRate.Value) 
                    : 0;
            
                var availableCredit = creditLimit + account.Balance;
                return availableCredit >= amount;
        
            default:
                throw new InvalidOperationException($"Unsupported account type: {account.Type}");
        }
    }
}