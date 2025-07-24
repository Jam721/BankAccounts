using MediatR;

namespace BankAccounts.Application.Commands;

public class DepositCommand : IRequest
{
    public Guid AccountId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = null!;
    public string Description { get; init; } = "Cash deposit";
}