using BelgradeATC.Core.Enums;

namespace BelgradeATC.Core.Entities;

public class ParkingSpot
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string SpotNumber { get; set; } = string.Empty;
  public AircraftType Type { get; set; }
  public string? OccupiedBy { get; set; }
}
