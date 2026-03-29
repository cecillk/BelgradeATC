using BelgradeATC.Core.Enums;
using BelgradeATC.Application.Interfaces;
using BelgradeATC.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using BelgradeATC.Application.Models;
using BelgradeATC.Application.Models.Requests;
using BelgradeATC.Application.Models.Responses;
using BelgradeATC.Core.Entities;

namespace BelgradeATC.Application.Services;

public class AircraftService : IAircraftService
{
    private readonly IAircraftRepository _aircraftRepository;
    private readonly IParkingSpotRepository _parkingSpotRepository;
    private readonly IStateChangeLogRepository _logRepository;
    private readonly ILogger<AircraftService> _logger;

    public AircraftService(
        IAircraftRepository aircraftRepository,
        IParkingSpotRepository parkingSpotRepository,
        IStateChangeLogRepository logRepository,
        ILogger<AircraftService> logger)
    {
        _aircraftRepository = aircraftRepository;
        _parkingSpotRepository = parkingSpotRepository;
        _logRepository = logRepository;
        _logger = logger;
    }

    public async Task<bool> UpdateLocationAsync(UpdateLocationRequest request)
    {

        var response = await _aircraftRepository.GetByCallSignAsync(request.CallSign);

        if (response == null)
        {
            return false;
        }

        response.Type = request.AircraftType;
        response.Longitude = request.Longitude;
        response.Altitude = request.Altitude;
        response.Heading = request.Heading;
        response.Latitude = request.Latitude;
        response.LastSeen = DateTime.UtcNow;

        await _aircraftRepository.SaveChangesAsync();
        return true;
    }

    public async Task<ProcesIntentResponse> ProcessIntentAsync(ProcesIntentRequest request)
    {
        var response = await _aircraftRepository.GetByCallSignAsync(request.CallSign);

        if (response != null)
        {
            var validTransitions = new Dictionary<AircraftState, AircraftState[]>
                {
                    { AircraftState.Parked,   new[] { AircraftState.TakeOff } },
                    { AircraftState.TakeOff,  new[] { AircraftState.Airborne } },
                    { AircraftState.Airborne, new[] { AircraftState.Approach } },
                    { AircraftState.Approach, new[] { AircraftState.Landed, AircraftState.Airborne } },
                    { AircraftState.Landed,   Array.Empty<AircraftState>() }
                };

            if (!validTransitions[response.State].Contains(request.RequestedState))
            {
                _logger.LogInformation($"Invalid state transition — aircraft cannot move from {response.State} to {request.RequestedState}");

                await _logRepository.AddAsync(new StateChangeLog
                {
                    AircraftId = response.Id,
                    RequestedState = request.RequestedState,
                    Outcome = LogOutcome.Rejected,
                    Reason = $"Invalid state transition — aircraft cannot move from {response.State} to {request.RequestedState}",
                    Timestamp = DateTime.UtcNow
                });

                await _logRepository.SaveChangesAsync();

                return new ProcesIntentResponse
                {
                    Success = false
                };
            }


            if (request.RequestedState == AircraftState.TakeOff)
            {
                var landed = await _aircraftRepository.AnyInStateAsync(AircraftState.Landed);
                var takeOff = await _aircraftRepository.AnyInStateAsync(AircraftState.TakeOff);

                if (!landed && !takeOff)
                {
                    response.State = AircraftState.TakeOff;
                    await _aircraftRepository.SaveChangesAsync();
                    _logger.LogInformation("Aircraft {CallSign} changed state to {State}", request.CallSign, request.RequestedState);

                    await _logRepository.AddAsync(new StateChangeLog
                    {
                        AircraftId = response.Id,
                        RequestedState = request.RequestedState,
                        Outcome = LogOutcome.Accepted,
                        Timestamp = DateTime.UtcNow
                    });

                    await _logRepository.SaveChangesAsync();
                    return new ProcesIntentResponse
                    {
                        Success = true
                    };
                }
            }

            if (request.RequestedState == AircraftState.Approach)
            {
                var landed = await _aircraftRepository.AnyInStateAsync(AircraftState.Landed);
                var takeOff = await _aircraftRepository.AnyInStateAsync(AircraftState.TakeOff);
                var approach = await _aircraftRepository.AnyInStateAsync(AircraftState.Approach);
                var hasParking = await _parkingSpotRepository.AvailableCountAsync(response.Type) > 0;


                if (!landed && !takeOff && !approach && hasParking)
                {
                    response.State = AircraftState.Approach;
                    await _aircraftRepository.SaveChangesAsync();
                    _logger.LogInformation("Aircraft {CallSign} changed state to {State}", request.CallSign, request.RequestedState);

                    await _logRepository.AddAsync(new StateChangeLog
                    {
                        AircraftId = response.Id,
                        RequestedState = request.RequestedState,
                        Outcome = LogOutcome.Accepted,
                        Timestamp = DateTime.UtcNow
                    });

                    await _logRepository.SaveChangesAsync();
                    return new ProcesIntentResponse
                    {
                        Success = true
                    };
                }
            }

            if (request.RequestedState == AircraftState.Airborne)
            {
                response.State = request.RequestedState;
                await _aircraftRepository.SaveChangesAsync();

                await _logRepository.AddAsync(new StateChangeLog
                {
                    AircraftId = response.Id,
                    RequestedState = request.RequestedState,
                    Outcome = LogOutcome.Accepted,
                    Timestamp = DateTime.UtcNow
                });

                await _logRepository.SaveChangesAsync();
                return new ProcesIntentResponse { Success = true };
            }

            if (request.RequestedState == AircraftState.Landed)
            {
                response.State = request.RequestedState;
                await _aircraftRepository.SaveChangesAsync();
                await _logRepository.AddAsync(new StateChangeLog
                {
                    AircraftId = response.Id,
                    RequestedState = request.RequestedState,
                    Outcome = LogOutcome.Accepted,
                    Timestamp = DateTime.UtcNow
                });

                await _logRepository.SaveChangesAsync();
                return new ProcesIntentResponse { Success = true };
            }


            await _logRepository.AddAsync(new StateChangeLog
            {
                AircraftId = response.Id,
                RequestedState = request.RequestedState,
                Outcome = LogOutcome.Rejected,
                Reason = "Runway busy or conditions not met",
                Timestamp = DateTime.UtcNow
            });

            await _logRepository.SaveChangesAsync();

            return new ProcesIntentResponse
            {
                Success = false
            };
        }

        return new ProcesIntentResponse
        {
            Success = false
        };


    }
}
