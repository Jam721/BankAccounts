using BankAccounts.Application.Commands;
using BankAccounts.Domain.Enums;
using BankAccounts.Domain.Interfaces;
using BankAccounts.Domain.Models;
using MediatR;

namespace BankAccounts.Application.CommandHandlers;

public class RegisterTransactionCommandHandler(
    IAccountRepository accountRepository,
    ITransactionRepository transactionRepository)
    : IRequestHandler<RegisterTransactionCommand>
{
    public async Task Handle(RegisterTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
    
        var newBalance = account.Balance + 
                         (request.Type == TransactionType.Credit ? request.Amount : -request.Amount);
    
        await accountRepository.UpdateBalanceAsync(account.Id, newBalance, cancellationToken);

        var transaction = new Transaction
        {
            AccountId = request.AccountId,
            CounterpartyAccountId = request.CounterpartyAccountId,
            Amount = request.Amount,
            Currency = request.Currency,
            Type = request.Type,
            Description = request.Description,
            DateTime = DateTime.UtcNow
        };
    
        await transactionRepository.AddAsync(transaction, cancellationToken);
    }
}