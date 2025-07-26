using AutoMapper;
using BankAccounts.Application.Dto;
using BankAccounts.Application.Queries;
using BankAccounts.Domain;
using BankAccounts.Domain.Interfaces;
using MediatR;

namespace BankAccounts.Application.QueryHandlers;

public class GetAccountsQueryHandler(
    IAccountRepository repository,
    IMapper mapper) : IRequestHandler<GetAccountsQuery, PaginatedResult<AccountDto>>
{
    public async Task<PaginatedResult<AccountDto>> Handle(
        GetAccountsQuery request, 
        CancellationToken cancellationToken)
    {
        var accounts = await repository.GetPaginatedAccountsAsync(
            request.PageSize,
            request.PageNumber,
            request.OwnerId,
            request.Type,
            request.Currency,
            request.MinBalance,
            request.MaxBalance,
            request.IsActive,
            cancellationToken);

        return new PaginatedResult<AccountDto>(
            mapper.Map<List<AccountDto>>(accounts.Items),
            accounts.TotalCount,
            accounts.PageNumber,
            accounts.PageSize);
    }
}