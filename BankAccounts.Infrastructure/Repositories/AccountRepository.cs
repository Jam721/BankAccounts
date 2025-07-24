using AutoMapper;
using BankAccounts.Domain;
using BankAccounts.Domain.Enums;
using BankAccounts.Domain.Interfaces;
using BankAccounts.Domain.Models;
using BankAccounts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITransactionRepository _transactionRepository;

    public AccountRepository(AppDbContext context, IMapper mapper, ITransactionRepository transactionRepository)
    {
        _context = context;
        _mapper = mapper;
        _transactionRepository = transactionRepository;
    }

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
        var query = _context.Accounts.AsQueryable();

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
            _mapper.Map<List<Account>>(accounts),
            totalCount, 
            pageNumber, 
            pageSize);
    }

    public async Task<Account> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var accountEntity = await _context.Accounts
            .FirstOrDefaultAsync(a=>a.Id == id, cancellationToken);
        
        return _mapper.Map<Account>(accountEntity);
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken)
    {
        var accountEntity = _mapper.Map<AccountEntity>(account);
        
        await _context.Accounts.AddAsync(accountEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateBalanceAsync(Guid id, decimal balance, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    
        if (account == null) 
            throw new Exception();

        account.Balance = balance;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        var existingEntity = await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == account.Id, cancellationToken);

        if (existingEntity == null)
            throw new Exception();

        _mapper.Map(account, existingEntity);
    
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts.FirstOrDefaultAsync(a=>a.Id==id, cancellationToken);
        if (entity != null)
        {
            _context.Accounts.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Accounts.AnyAsync(a => a.Id == id, cancellationToken);
    
    public async Task<IEnumerable<Account>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken)
    {
        var entities = await _context.Accounts
            .Where(a => a.OwnerId == ownerId)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<Account>>(entities);
    }

    public async Task ExecuteTransactionAsync(Guid accountId, decimal amount, string description, Guid? counterpartyId = null,
        CancellationToken cancellationToken = default)
    {
        var account = await GetByIdAsync(accountId, cancellationToken);
        account.Balance += amount;
        await UpdateAsync(account, cancellationToken);

        await _transactionRepository.AddAsync(new Transaction
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
        var account = await _context.Accounts.FirstOrDefaultAsync(a=>a.Id == id, cancellationToken);
        if (account is { Balance: 0 })
        {
            account.CloseDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
        else throw new InvalidOperationException(
            account == null ? "Account not found" : "Balance must be zero to close account");
    }
}