using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using BelgradeATC.Application.Interfaces;
using BelgradeATC.Application.Models.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BelgradeATC.Infrastructure.BackgroundServices;

public class WeatherService(ILogger<WeatherService> logger,
IHttpClientFactory httpclient,
IConfiguration config,
IWeatherStore weatherStore) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
      var apiKey = config["ExternalApis:Weather:ApiKey"];

      using var client = httpclient.CreateClient();
      var url = $"https://api.openweathermap.org/data/2.5/weather?q=Belgrade&appid={apiKey}";

      var response = await client.GetAsync(url);
      if (response.IsSuccessStatusCode)
      {
        var responseBody = await response.Content.ReadAsStringAsync();

        var resp = JsonSerializer.Deserialize<OpenWeatherMapResponse>(responseBody);

        if (resp != null)
        {
          WeatherResponse weatherResponse = new WeatherResponse
          {
            Description = resp.Weather[0].Description,
            Temperature = resp.Main.Temp - 273.15,
            Visibility = resp.Visibility,
            Wind = new WindResponse
            {
              Speed = resp.Wind.Speed,
              Deg = resp.Wind.Deg
            },
            LastUpdate = DateTimeOffset.FromUnixTimeSeconds(resp.Dt).ToString("yyyy-MM-dd HH:mm:sszzz")
          };


          weatherStore.Update(weatherResponse);
        }

        logger.LogInformation("Weather data updated");
      }
    }
  }
}
