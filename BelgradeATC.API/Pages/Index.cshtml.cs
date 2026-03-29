using BelgradeATC.Application.Interfaces;
using BelgradeATC.Application.Models.Responses;
using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BelgradeATC.Pages;

[Authorize(AuthenticationSchemes = "AdminCookie")]
public class IndexModel : PageModel
{
    private readonly IWeatherStore _weatherStore;
    private readonly IParkingSpotRepository _parkingRepository;
    private readonly IStateChangeLogRepository _statechangeRepository;

    public IndexModel(IWeatherStore weatherStore, IParkingSpotRepository parkingRepository, IStateChangeLogRepository statechangelogRepository)
    {
        _weatherStore = weatherStore;
        _parkingRepository = parkingRepository;
        _statechangeRepository = statechangelogRepository;
    }


    public WeatherResponse? Weather { get; set; }
    public List<ParkingSpot> ParkingSpots { get; set; } = new();
    public List<StateChangeLog> RecentLogs { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        Weather = _weatherStore.GetLatest();
        ParkingSpots = await _parkingRepository.GetAllAsync();
        RecentLogs = await _statechangeRepository.GetRecentAsync(10);

        return Page();
    }
}
