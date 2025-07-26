using BankAccounts.Application.Interfaces;

namespace BankAccounts.Application.Services;

public class StubCurrencyService : ICurrencyService
{
    private static readonly HashSet<string> SupportedCurrencies = ["RUB", "USD", "EUR", "GBP"];

    public bool IsSupportedCurrency(string currencyCode)
    {
        return SupportedCurrencies.Contains(currencyCode);
    }
}