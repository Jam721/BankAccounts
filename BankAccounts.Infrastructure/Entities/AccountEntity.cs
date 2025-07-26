using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BankAccounts.Domain.Enums;

namespace BankAccounts.Infrastructure.Entities;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class AccountEntity
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public AccountType Type { get; set; }
    
    [MaxLength(5)]
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public decimal? InterestRate { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public ICollection<TransactionEntity> Transactions { get; set; } = [];
}