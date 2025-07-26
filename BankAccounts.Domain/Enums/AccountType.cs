using System.Diagnostics.CodeAnalysis;

namespace BankAccounts.Domain.Enums;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum AccountType
{
    Checking = 0,
    Deposit = 1,
    Credit = 2
}