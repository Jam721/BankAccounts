using BankAccounts.Application.Interfaces;

namespace BankAccounts.Application.Services;

public class StubOwnerVerificationService : IOwnerVerificationService
{
    private static readonly HashSet<Guid> ExistingOwners =
    [
        Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Guid.Parse("22222222-2222-2222-2222-222222222222")
    ];

    public Task<bool> OwnerExistsAsync(Guid ownerId, CancellationToken cancellationToken)
    {
        return Task.FromResult(ExistingOwners.Contains(ownerId));
    }
}