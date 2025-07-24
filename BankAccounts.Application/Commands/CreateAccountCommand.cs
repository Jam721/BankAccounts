using BankAccounts.Domain.Enums;
using MediatR;

namespace BankAccounts.Application.Commands;

public class CreateAccountCommand : IRequest<Guid>
{
    public Guid OwnerId { get; set; }
    public AccountType Type { get; set; }
    public string Currency { get; set; } = "RUB";
    public decimal? InterestRate { get; set; }
}