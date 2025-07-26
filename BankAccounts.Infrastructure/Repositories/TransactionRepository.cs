using AutoMapper;
using BankAccounts.Domain.Interfaces;
using BankAccounts.Domain.Models;
using BankAccounts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Infrastructure.Repositories;

public class TransactionRepository(AppDbContext context, IMapper mapper) : ITransactionRepository
{
    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<TransactionEntity>(transaction);
        await context.Transactions.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Transaction>> GetByAccountIdAndPeriodAsync(
        Guid accountId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken)
    {
        var entities = context.Transactions;
        if (fromDate.HasValue && toDate.HasValue)
        {
            var res = await entities
                .Where(t => t.AccountId == accountId && t.DateTime >= fromDate && t.DateTime <= toDate)
                .ToListAsync(cancellationToken);
            
            return mapper.Map<IEnumerable<Transaction>>(res);
        }
        else
        {
            var res = await entities.ToListAsync(cancellationToken);
            
            return mapper.Map<IEnumerable<Transaction>>(res);
        }
    }
}