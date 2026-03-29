using System;
using System.Text.Json.Serialization;

namespace BelgradeATC.Application.Models.Responses;

public class WeatherResponse
{
  public string Description { get; set; }
  public double Temperature { get; set; }
  public int Visibility { get; set; }
  public WindResponse Wind { get; set; }
  public string LastUpdate { get; set; }

}

public class WindResponse
{
  public double Speed { get; set; }
  public int Deg { get; set; }

}
