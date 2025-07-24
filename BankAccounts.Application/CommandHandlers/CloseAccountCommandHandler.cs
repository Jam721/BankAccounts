using BankAccounts.Application.Commands;
using BankAccounts.Domain.Interfaces;
using MediatR;

namespace BankAccounts.Application.CommandHandlers;

public class CloseAccountCommandHandler(IAccountRepository repository) : IRequestHandler<CloseAccountCommand>
{
    public async Task Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        await repository.CloseAccountAsync(request.AccountId, cancellationToken);
    }
}