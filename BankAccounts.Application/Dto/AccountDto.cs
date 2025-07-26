namespace BankAccounts.Application.Dto;


public record AccountDto(
    Guid Id,
    Guid OwnerId,
    string Type,
    string Currency,
    decimal Balance,
    decimal? InterestRate,
    DateTime OpenDate,
    DateTime? CloseDate);