using BankAccounts.Application.Commands;
using BankAccounts.Domain.Enums;
using BankAccounts.Domain.Interfaces;
using BankAccounts.Domain.Models;
using MediatR;

namespace BankAccounts.Application.CommandHandlers;

public class CreateAccountCommandHandler(IAccountRepository repository) : IRequestHandler<CreateAccountCommand, Guid>
{
    public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = new Account
        {
            Id = Guid.NewGuid(),
            OwnerId = request.OwnerId,
            Type = request.Type,
            Currency = request.Currency,
            Balance = 0,
            InterestRate = request.Type != AccountType.Checking ? request.InterestRate : null,
            OpenDate = DateTime.UtcNow
        };
        
        await repository.AddAsync(account, cancellationToken);
        return account.Id;
    }
}