﻿using System.Diagnostics.CodeAnalysis;
using BankAccounts.Domain.Enums;

namespace BankAccounts.Domain.Models;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CounterpartyAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
}