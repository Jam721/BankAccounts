using BankAccounts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }
    
    
}