using BankAccounts.Domain.Models;

namespace BankAccounts.Domain.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(
        Transaction transaction, CancellationToken cancellationToken);
    
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(
        Guid accountId, CancellationToken cancellationToken);

    Task<IEnumerable<Transaction>> GetByAccountIdAndPeriodAsync(
        Guid accountId, DateTime? from, DateTime? to, CancellationToken cancellationToken);
}