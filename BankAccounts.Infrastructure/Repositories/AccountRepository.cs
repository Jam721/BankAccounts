using AutoMapper;
using BankAccounts.Domain;
using BankAccounts.Domain.Enums;
using BankAccounts.Domain.Interfaces;
using BankAccounts.Domain.Models;
using BankAccounts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Infrastructure.Repositories;

public class AccountRepository(AppDbContext context, IMapper mapper, ITransactionRepository transactionRepository)
    : IAccountRepository
{
    public async Task<PaginatedResult<Account>> GetPaginatedAccountsAsync(
        int pageSize,
        int pageNumber,
        Guid? ownerId,
        AccountType? type,
        string? currency,
        decimal? minBalance,
        decimal? maxBalance,
        bool? isActive,
        CancellationToken cancellationToken)
    {
        var query = context.Accounts.AsQueryable();

        if (ownerId.HasValue) 
            query = query.Where(a => a.OwnerId == ownerId);
        
        if (type.HasValue) 
            query = query.Where(a => a.Type == type);
        
        if (!string.IsNullOrEmpty(currency)) 
            query = query.Where(a => a.Currency == currency);
        
        if (minBalance.HasValue) 
            query = query.Where(a => a.Balance >= minBalance);
        
        if (maxBalance.HasValue) 
            query = query.Where(a => a.Balance <= maxBalance);
        
        if (isActive.HasValue)
            query = isActive.Value 
                ? query.Where(a => a.CloseDate == null) 
                : query.Where(a => a.CloseDate != null);

        var totalCount = await query.CountAsync(cancellationToken);

        var accounts = await query
            .OrderBy(a => a.OpenDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<Account>(
            mapper.Map<List<Account>>(accounts),
            totalCount, 
            pageNumber, 
            pageSize);
    }

    public async Task<Account> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var accountEntity = await context.Accounts
            .FirstOrDefaultAsync(a=>a.Id == id, cancellationToken);
        
        return mapper.Map<Account>(accountEntity);
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken)
    {
        var accountEntity = mapper.Map<AccountEntity>(account);
        
        await context.Accounts.AddAsync(accountEntity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateBalanceAsync(Guid id, decimal balance, CancellationToken cancellationToken)
    {
        var account = await context.Accounts
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    
        if (account == null) 
            throw new InvalidOperationException("Account not found");

        account.Balance = balance;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        var existingEntity = await context.Accounts
            .FirstOrDefaultAsync(a => a.Id == account.Id, cancellationToken);

        if (existingEntity == null)
            throw new InvalidOperationException("Account not found");

        mapper.Map(account, existingEntity);
    
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task ExecuteTransactionAsync(Guid accountId, decimal amount, string description, Guid? counterpartyId = null,
        CancellationToken cancellationToken = default)
    {
        var account = await GetByIdAsync(accountId, cancellationToken);
        account.Balance += amount;
        await UpdateAsync(account, cancellationToken);

        await transactionRepository.AddAsync(new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            CounterpartyAccountId = counterpartyId,
            Amount = Math.Abs(amount),
            Currency = account.Currency,
            Type = amount > 0 ? TransactionType.Credit : TransactionType.Debit,
            Description = description,
            DateTime = DateTime.UtcNow
        }, cancellationToken);
    }
    

    public async Task CloseAccountAsync(Guid id, CancellationToken cancellationToken)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(a=>a.Id == id, cancellationToken);
        if (account is { Balance: 0 })
        {
            account.CloseDate = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
        }
        else throw new InvalidOperationException(
            account == null ? "Account not found" : "Balance must be zero to close account");
    }
}