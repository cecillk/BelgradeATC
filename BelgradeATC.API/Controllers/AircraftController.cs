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
    [HttpPut("api/{callSign}/location")]
    public async Task<IActionResult> UpdateLocation(string callSign, [FromBody] UpdateLocationCommand command)
    {
        command.CallSign = callSign;
        var success = await mediator.Send(command);

        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPost("api/{callSign}/intent")]
    public async Task<IActionResult> ProcessIntent(string callSign, [FromBody] ProcessIntentCommand command)
    {
        command.CallSign = callSign;
        var result = await mediator.Send(command);

        if (!result.Success) return Conflict();
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("/api/public/weather")]
    public IActionResult Weather()
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
