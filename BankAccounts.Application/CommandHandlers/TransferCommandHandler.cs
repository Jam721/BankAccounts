using BankAccounts.Application.Commands;
using BankAccounts.Domain.Interfaces;
using BankAccounts.Infrastructure;
using MediatR;

namespace BankAccounts.Application.CommandHandlers;

public class TransferCommandHandler(
    IAccountRepository accountRepo,
    AppDbContext context) : IRequestHandler<TransferCommand>
{

    public async Task Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await accountRepo.ExecuteTransactionAsync(
                request.FromAccountId, 
                -request.Amount,
                $"Transfer to account {request.ToAccountId}",
                request.ToAccountId, cancellationToken);

            await accountRepo.ExecuteTransactionAsync(
                request.ToAccountId, 
                request.Amount,
                $"Transfer from account {request.FromAccountId}",
                request.FromAccountId, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}