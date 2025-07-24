using BankAccounts.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankAccounts.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RegisterTransaction([FromBody] RegisterTransactionCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    
}