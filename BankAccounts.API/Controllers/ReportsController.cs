using BankAccounts.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankAccounts.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(ITransactionRepository repository) : ControllerBase
{
    [HttpGet("{accountId:guid}")]
    public async Task<IActionResult> GetStatement(
        Guid accountId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        CancellationToken cancellationToken)
    {
        return Ok(await repository.GetByAccountIdAndPeriodAsync(accountId, fromDate, toDate, cancellationToken));
    }
}