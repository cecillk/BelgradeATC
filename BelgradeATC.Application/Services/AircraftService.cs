using BelgradeATC.Core.Enums;
using BelgradeATC.Application.Interfaces;
using BelgradeATC.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using BelgradeATC.Application.Models.Requests;
using BelgradeATC.Application.Models.Responses;
using BelgradeATC.Core.Entities;

namespace BelgradeATC.Application.Services;

public class AircraftService(
    IAircraftRepository aircraftRepository,
    IParkingSpotRepository parkingSpotRepository,
    IStateChangeLogRepository logRepository,
    ILogger<AircraftService> logger)
    : IAircraftService
{
    public async Task<bool> UpdateLocationAsync(UpdateLocationRequest request)
    {

        var response = await aircraftRepository.GetByCallSignAsync(request.CallSign);

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

        await aircraftRepository.SaveChangesAsync();
        return true;
    }

    public async Task<ProcesIntentResponse> ProcessIntentAsync(ProcessIntentRequest request)
    {
        var response = await aircraftRepository.GetByCallSignAsync(request.CallSign);

        if (response == null)
            return new ProcesIntentResponse
            {
                Success = false
            };
        var validTransitions = new Dictionary<AircraftState, AircraftState[]>
        {
            { AircraftState.Parked, [AircraftState.TakeOff] },
            { AircraftState.TakeOff, [AircraftState.Airborne] },
            { AircraftState.Airborne, [AircraftState.Approach] },
            { AircraftState.Approach, [AircraftState.Landed, AircraftState.Airborne] },
            { AircraftState.Landed,   [] }
        };

        if (!validTransitions[response.State].Contains(request.RequestedState))
        {
            logger.LogInformation("Invalid state transition — aircraft cannot move from {ResponseState} to {RequestRequestedState}", response.State, request.RequestedState);

            await logRepository.AddAsync(new StateChangeLog
            {
                AircraftId = response.Id,
                RequestedState = request.RequestedState,
                Outcome = LogOutcome.Rejected,
                Reason = $"Invalid state transition — aircraft cannot move from {response.State} to {request.RequestedState}",
                Timestamp = DateTime.UtcNow
            });

            await logRepository.SaveChangesAsync();

            return new ProcesIntentResponse
            {
                Success = false
            };
        }


        switch (request.RequestedState)
        {
            case AircraftState.TakeOff:
            {
                var landed = await aircraftRepository.AnyInStateAsync(AircraftState.Landed);
                var takeOff = await aircraftRepository.AnyInStateAsync(AircraftState.TakeOff);

                if (!landed && !takeOff)
                {
                    response.State = AircraftState.TakeOff;
                    await aircraftRepository.SaveChangesAsync();
                    logger.LogInformation("Aircraft {CallSign} changed state to {State}", request.CallSign, request.RequestedState);

                    await logRepository.AddAsync(new StateChangeLog
                    {
                        AircraftId = response.Id,
                        RequestedState = request.RequestedState,
                        Outcome = LogOutcome.Accepted,
                        Timestamp = DateTime.UtcNow
                    });

                    await logRepository.SaveChangesAsync();
                    return new ProcesIntentResponse
                    {
                        Success = true
                    };
                }

                break;
            }
            case AircraftState.Approach:
            {
                var landed = await aircraftRepository.AnyInStateAsync(AircraftState.Landed);
                var takeOff = await aircraftRepository.AnyInStateAsync(AircraftState.TakeOff);
                var approach = await aircraftRepository.AnyInStateAsync(AircraftState.Approach);
                var hasParking = await parkingSpotRepository.AvailableCountAsync(response.Type) > 0;


                if (!landed && !takeOff && !approach && hasParking)
                {
                    response.State = AircraftState.Approach;
                    await aircraftRepository.SaveChangesAsync();
                    logger.LogInformation("Aircraft {CallSign} changed state to {State}", request.CallSign, request.RequestedState);

                    await logRepository.AddAsync(new StateChangeLog
                    {
                        AircraftId = response.Id,
                        RequestedState = request.RequestedState,
                        Outcome = LogOutcome.Accepted,
                        Timestamp = DateTime.UtcNow
                    });

                    await logRepository.SaveChangesAsync();
                    return new ProcesIntentResponse
                    {
                        Success = true
                    };
                }

                break;
            }
            case AircraftState.Airborne:
            case AircraftState.Landed:
                response.State = request.RequestedState;
                await aircraftRepository.SaveChangesAsync();

                await logRepository.AddAsync(new StateChangeLog
                {
                    AircraftId = response.Id,
                    RequestedState = request.RequestedState,
                    Outcome = LogOutcome.Accepted,
                    Timestamp = DateTime.UtcNow
                });

                await logRepository.SaveChangesAsync();
                return new ProcesIntentResponse { Success = true };
            case AircraftState.Parked:
                break;
            default:
                throw new Exception($"Unknown aircraft state {request.RequestedState}");
        }


        await logRepository.AddAsync(new StateChangeLog
        {
            AircraftId = response.Id,
            RequestedState = request.RequestedState,
            Outcome = LogOutcome.Rejected,
            Reason = "Runway busy or conditions not met",
            Timestamp = DateTime.UtcNow
        });

        await logRepository.SaveChangesAsync();

        return new ProcesIntentResponse
        {
            Success = false
        };


    }
}
