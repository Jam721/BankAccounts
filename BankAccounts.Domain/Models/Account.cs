﻿using BankAccounts.Domain.Enums;

namespace BankAccounts.Domain.Models;

public class Account
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public AccountType Type { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public decimal? InterestRate { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = [];
}