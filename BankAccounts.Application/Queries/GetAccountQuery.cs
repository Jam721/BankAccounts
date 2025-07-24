using BankAccounts.Domain.Models;
using MediatR;

namespace BankAccounts.Application.Queries;

public class GetAccountQuery : IRequest<Account>
{
    public Guid AccountId { get; set; }
}