using BankAccounts.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankAccounts.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ITransactionRepository _repository;

    public ReportsController(ITransactionRepository repository) 
        => _repository = repository;

    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetStatement(
        Guid accountId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken cancellationToken)
    {
        return Ok(await _repository.GetByAccountIdAndPeriodAsync(accountId, from, to, cancellationToken));
    }
}