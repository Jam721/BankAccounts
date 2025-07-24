using AutoMapper;
using BankAccounts.Application.Dto;
using BankAccounts.Application.Queries;
using BankAccounts.Domain;
using BankAccounts.Domain.Interfaces;
using MediatR;

namespace BankAccounts.Application.QueryHandlers;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, PaginatedResult<AccountDto>>
{
    private readonly IAccountRepository _repository;
    private readonly IMapper _mapper;

    public GetAccountsQueryHandler(
        IAccountRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<AccountDto>> Handle(
        GetAccountsQuery request, 
        CancellationToken cancellationToken)
    {
        var accounts = await _repository.GetPaginatedAccountsAsync(
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
            _mapper.Map<List<AccountDto>>(accounts.Items),
            accounts.TotalCount,
            accounts.PageNumber,
            accounts.PageSize);
    }
}