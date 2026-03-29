using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Enums;
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

    SyncSpots(AircraftType.Airliner, airlinerSpots);
    SyncSpots(AircraftType.Private, privateSpots);

    if (dbcontext.ChangeTracker.HasChanges())
      await dbcontext.SaveChangesAsync();
  }

  private void SyncSpots(AircraftType type, int configuredCount)
  {
    var existing = dbcontext.ParkingSpots
      .Where(s => s.Type == type)
      .OrderBy(s => s.SpotNumber)
      .ToList();

    // Adding the missing spots
    for (int i = existing.Count + 1; i <= configuredCount; i++)
    {
      dbcontext.ParkingSpots.Add(new ParkingSpot
      {
        SpotNumber = GenerateSpotNumber(type, i),
        Type = type,
        OccupiedBy = null
      });
    }

    if (existing.Count > configuredCount)
    {
      var excess = existing.Skip(configuredCount).Where(s => s.OccupiedBy == null);
      dbcontext.ParkingSpots.RemoveRange(excess);
    }
  }

  public static string GenerateSpotNumber(AircraftType type, int index)
  {
    var prefix = type == AircraftType.Airliner ? "A" : "P";
    return $"{prefix}{index}";
  }
}

