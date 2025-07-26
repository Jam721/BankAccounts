using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BankAccounts.Domain.Enums;

namespace BankAccounts.Infrastructure.Entities;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class TransactionEntity
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CounterpartyAccountId { get; set; }
    public decimal Amount { get; set; }
    
    [MaxLength(5)]
    public string Currency { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
}