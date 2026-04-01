using System;
using BelgradeATC.Core.Enums;
using BelgradeATC.Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BelgradeATC.Infrastructure.BackgroundServices;

public class GroundCrewService(IServiceScopeFactory factory, ILogger<GroundCrewService> logger) : BackgroundService
{
  protected async override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      try { await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); }
      catch (TaskCanceledException) { break; }
      using var scope = factory.CreateScope();
      var _aircraftRepository = scope.ServiceProvider.GetRequiredService<IAircraftRepository>();
      var _parkingSpotRepository = scope.ServiceProvider.GetRequiredService<IParkingSpotRepository>();

      var aircrafts = await _aircraftRepository.GetAllInStateAsync(AircraftState.Landed);

      foreach (var aircraft in aircrafts)
      {
        var spot = await _parkingSpotRepository.GetFirstAvailableAsync(aircraft.Type);

        if (spot != null)
        {
          spot.OccupiedBy = aircraft.CallSign;
          aircraft.State = AircraftState.Parked;
          logger.LogInformation("Aircraft {CallSign} parked at spot {SpotNumber}", aircraft.CallSign, spot.SpotNumber);

          await _parkingSpotRepository.SaveChangesAsync();
          await _aircraftRepository.SaveChangesAsync();
        }
      }
    }
  }

}
