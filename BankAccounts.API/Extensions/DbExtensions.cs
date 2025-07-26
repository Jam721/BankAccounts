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

    public static void AddDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
    }
}