using BankAccounts.Application.Commands;
using BankAccounts.Domain.Enums;
using BankAccounts.Domain.Interfaces;
using BankAccounts.Domain.Models;
using MediatR;

namespace BankAccounts.Application.CommandHandlers;

public class DepositCommandHandler(
    IAccountRepository accountRepository,
    ITransactionRepository transactionRepository)  : IRequestHandler<DepositCommand>
{
    public async Task Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        
        account.Balance += request.Amount;
        
        var transaction = new Transaction
        {
            AccountId = request.AccountId,
            Amount = request.Amount,
            Currency = request.Currency,
            Type = TransactionType.Credit,
            Description = request.Description,
            DateTime = DateTime.UtcNow
        };
        
        await accountRepository.UpdateAsync(account, cancellationToken);
        await transactionRepository.AddAsync(transaction, cancellationToken);
    }
}