using System;
using BelgradeATC.Core.Enums;

namespace BelgradeATC.Application.Models.Requests;

public class UpdateLocationRequest
{
  public required string CallSign { get; set; }
  public AircraftType AircraftType { get; set; }
  public decimal Longitude { get; set; }
  public int Altitude { get; set; }
  public int Heading { get; set; }
  public decimal Latitude { get; set; }

}
