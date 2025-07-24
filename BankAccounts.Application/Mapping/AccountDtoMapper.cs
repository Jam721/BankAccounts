using AutoMapper;
using BankAccounts.Application.Dto;
using BankAccounts.Domain.Models;
using BankAccounts.Infrastructure.Entities;

namespace BankAccounts.Application.Mapping;

public class AccountDtoMapper : Profile
{
    public AccountDtoMapper()
    {
        CreateMap<Account, AccountDto>().ForMember(dest => dest.Type, 
            opt => opt.MapFrom(src => src.Type.ToString()));
        CreateMap<AccountEntity, AccountDto>()
            .ForMember(dest => dest.Type, 
                opt => opt.MapFrom(src => src.Type.ToString()));
    }
}