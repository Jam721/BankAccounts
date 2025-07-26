namespace BankAccounts.Application.Interfaces;

public interface IOwnerVerificationService
{
    Task<bool> OwnerExistsAsync(Guid ownerId, CancellationToken cancellationToken);
}