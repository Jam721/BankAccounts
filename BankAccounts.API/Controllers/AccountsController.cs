using BankAccounts.Application.Commands;
using BankAccounts.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankAccounts.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAccounts([FromQuery] GetAccountsQuery query)
    {
        return Ok(await mediator.Send(query));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccount(Guid id)
    {
        return Ok(await mediator.Send(new GetAccountQuery { AccountId = id }));
    } 
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateRepository([FromBody] CreateAccountCommand command)
    {
        return Ok(await mediator.Send(command));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> CloseAccount(Guid id)
    {
        await mediator.Send(new CloseAccountCommand { AccountId = id });
        return NoContent();
    }
}