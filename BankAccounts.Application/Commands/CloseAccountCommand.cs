using MediatR;

namespace BankAccounts.Application.Commands;

public class CloseAccountCommand : IRequest
{
    public Guid AccountId { get; set; }
}