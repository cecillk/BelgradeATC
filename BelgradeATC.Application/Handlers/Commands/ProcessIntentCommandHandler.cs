using BelgradeATC.Application.Commands;
using BelgradeATC.Application.Interfaces;
using BelgradeATC.Application.Models.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BelgradeATC.Application.Commands;

public class ProcessIntentCommandHandler : IRequestHandler<ProcessIntentCommand, ProcessIntentResult>
{
    private readonly IAircraftService _aircraftService;
    private readonly ILogger<ProcessIntentCommandHandler> _logger;

    public ProcessIntentCommandHandler(IAircraftService aircraftService, ILogger<ProcessIntentCommandHandler> logger)
    {
        _aircraftService = aircraftService;
        _logger = logger;
    }

    public async Task<ProcessIntentResult> Handle(ProcessIntentCommand command, CancellationToken cancellationToken)
    {
        ProcesIntentRequest request = new ProcesIntentRequest
        {
            CallSign = command.CallSign,
            RequestedState = command.RequestedState
        };

        var response = await _aircraftService.ProcessIntentAsync(request);

        return new ProcessIntentResult
        {
            Success = response.Success
        };

    }
}
