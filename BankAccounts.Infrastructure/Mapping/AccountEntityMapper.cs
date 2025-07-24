using AutoMapper;
using BankAccounts.Domain.Models;
using BankAccounts.Infrastructure.Entities;

namespace BankAccounts.Infrastructure.Mapping;

public class AccountEntityMapper : Profile
{
    public AccountEntityMapper()
    {
        CreateMap<Account, AccountEntity>();
        CreateMap<AccountEntity, Account>();
    }
}