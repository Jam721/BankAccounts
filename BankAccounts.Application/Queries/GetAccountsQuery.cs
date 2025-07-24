using BankAccounts.Application.Dto;
using BankAccounts.Domain;
using BankAccounts.Domain.Enums;
using MediatR;

namespace BankAccounts.Application.Queries;

public class GetAccountsQuery : IRequest<PaginatedResult<AccountDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? OwnerId { get; set; }
    public AccountType? Type { get; set; }
    public string? Currency { get; set; }
    public decimal? MinBalance { get; set; }
    public decimal? MaxBalance { get; set; }
    public bool? IsActive { get; set; }
}