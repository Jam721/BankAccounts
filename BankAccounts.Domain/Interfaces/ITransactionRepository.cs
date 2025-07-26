using BankAccounts.Domain.Models;

namespace BankAccounts.Domain.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(
        Transaction transaction, CancellationToken cancellationToken);

    Task<IEnumerable<Transaction>> GetByAccountIdAndPeriodAsync(
        Guid accountId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken);
}