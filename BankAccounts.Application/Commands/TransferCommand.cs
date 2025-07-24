using MediatR;

namespace BankAccounts.Application.Commands;

public class TransferCommand : IRequest
{
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "RUB";
}