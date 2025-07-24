using BankAccounts.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.API.Extensions;

public static class DbExtensions
{
    public static void AddDbContextExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });
    }
}