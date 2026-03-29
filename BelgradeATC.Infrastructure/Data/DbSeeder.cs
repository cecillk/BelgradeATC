using System;
using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Enums;
using BelgradeATC.Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace BelgradeATC.Infrastructure.Data;

public class DbSeeder(IConfiguration config, AppDbContext dbcontext)
{
  public async Task Process()
  {

    var airlinerSpots = int.Parse(config["ParkingConfig:AirlinerSpots"]);
    var privateSpots = int.Parse(config["ParkingConfig:PrivateSpots"]);

    if (airlinerSpots <= 0 || privateSpots <= 0)
      throw new InvalidOperationException("ParkingConfig values must be greater than 0.");

    if (!dbcontext.ParkingSpots.Any())
    {
      for (int i = 1; i <= airlinerSpots; i++)
      {
        var spot = new ParkingSpot
        {
          SpotNumber = GenerateSpotNumber(AircraftType.Airliner, i),
          Type = AircraftType.Airliner,
          OccupiedBy = null
        };

        dbcontext.ParkingSpots.Add(spot);
      }

      for (int i = 1; i <= privateSpots; i++)
      {
        var spot = new ParkingSpot
        {
          SpotNumber = GenerateSpotNumber(AircraftType.Private, i),
          Type = AircraftType.Private,
          OccupiedBy = null
        };

        dbcontext.ParkingSpots.Add(spot);
      }

      await dbcontext.SaveChangesAsync();

    }
  }





  public static string GenerateSpotNumber(AircraftType type, int index)
  {
    var prefix = type == AircraftType.Airliner ? "A" : "P";
    return $"{prefix}{index}";
  }
}

