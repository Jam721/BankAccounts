using BankAccounts.Application.Dto;
using BankAccounts.Application.Mapping;
using BankAccounts.Infrastructure.Mapping;

namespace BankAccounts.API.Extensions;

public static class MapperExtensions
{
    public static void AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AccountEntityMapper).Assembly);
        services.AddAutoMapper(typeof(TransactionEntityMapper).Assembly);
        services.AddAutoMapper(typeof(AccountDtoMapper).Assembly);
    }
}