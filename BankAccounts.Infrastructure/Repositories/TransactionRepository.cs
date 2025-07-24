using AutoMapper;
using BankAccounts.Domain.Interfaces;
using BankAccounts.Domain.Models;
using BankAccounts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TransactionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<TransactionEntity>(transaction);
        await _context.Transactions.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var entities = await _context.Transactions
            .Where(t => t.AccountId == accountId)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<Transaction>>(entities);
    }
    
    public async Task<IEnumerable<Transaction>> GetByAccountIdAndPeriodAsync(
        Guid accountId, DateTime? from, DateTime? to, CancellationToken cancellationToken)
    {
        var entities = _context.Transactions;
        if (from.HasValue && to.HasValue)
        {
            var res = await entities
                .Where(t => t.AccountId == accountId && t.DateTime >= from && t.DateTime <= to)
                .ToListAsync(cancellationToken);
            
            return _mapper.Map<IEnumerable<Transaction>>(res);
        }
        else
        {
            var res = await entities.ToListAsync(cancellationToken);
            
            return _mapper.Map<IEnumerable<Transaction>>(res);
        }
    }
}