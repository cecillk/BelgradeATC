using System;
using BelgradeATC.Application.Interfaces;
using BelgradeATC.Application.Models.Responses;
namespace BelgradeATC.Infrastructure;

public class WeatherStore : IWeatherStore
{
  private WeatherResponse? _latest;

  public WeatherResponse? GetLatest() => _latest;

  public void Update(WeatherResponse request)
  {
    _latest = request;
  }
}
