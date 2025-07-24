using AutoMapper;
using BankAccounts.Domain.Models;
using BankAccounts.Infrastructure.Entities;

namespace BankAccounts.Infrastructure.Mapping;

public class TransactionEntityMapper : Profile
{
    public TransactionEntityMapper()
    {
        CreateMap<Transaction, TransactionEntity>();
        CreateMap<TransactionEntity, Transaction>();
    }
}