using BankAccounts.Application.Interfaces;
using BankAccounts.Application.Services;

namespace BankAccounts.API.Extensions;

public static class ServicesExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IOwnerVerificationService, StubOwnerVerificationService>();
        services.AddScoped<ICurrencyService, StubCurrencyService>();
    }
}