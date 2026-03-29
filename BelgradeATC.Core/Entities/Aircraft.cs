using BelgradeATC.Core.Enums;

namespace BelgradeATC.Core.Entities;

public class Aircraft
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string CallSign { get; set; } = string.Empty;  // unique identifier e.g. NC9574
  public string PublicKey { get; set; } = string.Empty; // RSA public key in PEM format
  public AircraftType Type { get; set; }
  public AircraftState State { get; set; }
  public decimal? Latitude { get; set; }
  public decimal? Longitude { get; set; }
  public int? Altitude { get; set; }
  public int? Heading { get; set; }
  public DateTime LastSeen { get; set; }
}

