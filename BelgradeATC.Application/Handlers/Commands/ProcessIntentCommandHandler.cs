using BelgradeATC.Application.Commands;
using BelgradeATC.Application.Interfaces;
using BelgradeATC.Application.Models.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BelgradeATC.Application.Handlers.Commands;

public class ProcessIntentCommandHandler(IAircraftService aircraftService, ILogger<ProcessIntentCommandHandler> logger)
    : IRequestHandler<ProcessIntentCommand, ProcessIntentResult>
{
    private readonly ILogger<ProcessIntentCommandHandler> _logger = logger;

    public async Task<ProcessIntentResult> Handle(ProcessIntentCommand command, CancellationToken cancellationToken)
    {
        var request = new ProcessIntentRequest
        {
            CallSign = command.CallSign,
            RequestedState = command.RequestedState
        };

        var response = await aircraftService.ProcessIntentAsync(request);

        return new ProcessIntentResult
        {
            Success = response.Success
        };

    }
}
