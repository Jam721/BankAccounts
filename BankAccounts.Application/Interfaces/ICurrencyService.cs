namespace BankAccounts.Application.Interfaces;

public interface ICurrencyService
{
    bool IsSupportedCurrency(string currencyCode);
}