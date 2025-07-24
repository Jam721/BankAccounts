using BankAccounts.Domain.Interfaces;
using BankAccounts.Infrastructure.Repositories;

namespace BankAccounts.API.Extensions;

public static class RepositoriesExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    }
}