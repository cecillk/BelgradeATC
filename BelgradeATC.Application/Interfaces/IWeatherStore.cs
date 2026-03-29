using System;
using BelgradeATC.Application.Models.Responses;

namespace BelgradeATC.Application.Interfaces;

public interface IWeatherStore
{
  WeatherResponse? GetLatest();
  void Update(WeatherResponse request);
}
