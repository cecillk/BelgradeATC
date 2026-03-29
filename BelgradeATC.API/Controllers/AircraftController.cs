using BelgradeATC.Application.Commands;
using BelgradeATC.Application.Interfaces;
using BelgradeATC.Application.Queries;
using BelgradeATC.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BelgradeATC.API.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = AircraftAuthHandler.SchemeName)]
public class AircraftController(IMediator mediator, IWeatherStore weatherStore) : ControllerBase
{
    [HttpPut("api/{call_sign}/location")]
    public async Task<IActionResult> UpdateLocation(string call_sign, [FromBody] UpdateLocationCommand command)
    {
        command.CallSign = call_sign;
        var success = await mediator.Send(command);

        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPost("api/{call_sign}/intent")]
    public async Task<IActionResult> ProcessIntent(string call_sign, [FromBody] ProcessIntentCommand command)
    {
        command.CallSign = call_sign;
        var result = await mediator.Send(command);

        if (!result.Success) return Conflict();
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("/api/public/{call_sign}/weather")]
    public IActionResult Weather(string call_sign)
    {
        var request = weatherStore.GetLatest();
        if (request == null)
        {
            return NotFound();
        }

        return Ok(request);
    }

    [AllowAnonymous]
    [HttpGet("/api/logs/recent")]
    public async Task<IActionResult> RecentLogs()
    {
        var response = await mediator.Send(new GetRecentLogsQuery());

        return Ok(response);
    }


}
