using System;
using System.Text.Json.Serialization;

namespace BelgradeATC.Application.Models.Responses;

public class OpenWeatherMapResponse
{
  [JsonPropertyName("weather")]
  public List<WeatherDescription> Weather { get; set; }

  [JsonPropertyName("main")]
  public Main Main { get; set; }

  [JsonPropertyName("visibility")]
  public int Visibility { get; set; }

  [JsonPropertyName("wind")]
  public Wind Wind { get; set; }

  [JsonPropertyName("dt")]
  public long Dt { get; set; }
}

public class WeatherDescription
{
  [JsonPropertyName("description")]
  public string Description { get; set; } = string.Empty;
}

public class Main
{
  [JsonPropertyName("temp")]
  public double Temp { get; set; }
}

public class Wind
{
  [JsonPropertyName("speed")]
  public double Speed { get; set; }
  [JsonPropertyName("deg")]
  public int Deg { get; set; }
}
