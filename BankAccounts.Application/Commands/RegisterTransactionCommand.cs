using BankAccounts.Domain.Enums;
using MediatR;

namespace BankAccounts.Application.Commands;

public class RegisterTransactionCommand : IRequest
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "RUB";
    public TransactionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid? CounterpartyAccountId { get; set; }
}