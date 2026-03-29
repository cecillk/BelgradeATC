using BelgradeATC.Application.Commands;
using BelgradeATC.Application.Interfaces;
using BelgradeATC.Application.Models.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BelgradeATC.Application.Commands;

public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, bool>
{
    private readonly IAircraftService _aircraftService;
    private readonly ILogger<UpdateLocationCommandHandler> _logger;

    public UpdateLocationCommandHandler(IAircraftService aircraftService, ILogger<UpdateLocationCommandHandler> logger)
    {
        _aircraftService = aircraftService;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateLocationCommand command, CancellationToken cancellationToken)
    {
        UpdateLocationRequest request = new UpdateLocationRequest
        {
            CallSign = command.CallSign,
            AircraftType = command.Type,
            Longitude = command.Longitude,
            Altitude = command.Altitude,
            Heading = command.Heading,
            Latitude = command.Latitude
        };

        return await _aircraftService.UpdateLocationAsync(request);

    }
}
