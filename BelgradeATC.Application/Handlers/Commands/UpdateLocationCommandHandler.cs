using BelgradeATC.Application.Commands;
using BelgradeATC.Application.Interfaces;
using BelgradeATC.Application.Models.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BelgradeATC.Application.Handlers.Commands;

public class UpdateLocationCommandHandler(
    IAircraftService aircraftService,
    ILogger<UpdateLocationCommandHandler> logger)
    : IRequestHandler<UpdateLocationCommand, bool>
{
    private readonly ILogger<UpdateLocationCommandHandler> _logger = logger;

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

        return await aircraftService.UpdateLocationAsync(request);

    }
}
