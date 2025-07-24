using BankAccounts.Domain.Enums;
using BankAccounts.Domain.Models;

namespace BankAccounts.Domain.Interfaces;

public interface IAccountRepository
{
    Task<PaginatedResult<Account>> GetPaginatedAccountsAsync(
        int pageSize,
        int pageNumber,
        Guid? ownerId,
        AccountType? type,
        string? currency,
        decimal? minBalance,
        decimal? maxBalance,
        bool? isActive,
        CancellationToken cancellationToken);
    Task<Account> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Account account, CancellationToken cancellationToken);
    Task UpdateBalanceAsync(Guid id, decimal balance, CancellationToken cancellationToken);
    Task UpdateAsync(Account account, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Account>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken);
    Task ExecuteTransactionAsync(
        Guid accountId, decimal amount, 
        string description, Guid? counterpartyId = null,
        CancellationToken cancellationToken = default);
    Task CloseAccountAsync(Guid id, CancellationToken cancellationToken);

}