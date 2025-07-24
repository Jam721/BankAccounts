using BankAccounts.Application.Queries;
using BankAccounts.Domain.Interfaces;
using BankAccounts.Domain.Models;
using MediatR;

namespace BankAccounts.Application.QueryHandlers;

public class GetAccountQueryHandler(IAccountRepository repository) : IRequestHandler<GetAccountQuery, Account>
{
    public async Task<Account> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        => await repository.GetByIdAsync(request.AccountId, cancellationToken);
}