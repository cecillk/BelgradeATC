using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Enums;

namespace BelgradeATC.Core.Interfaces.Repositories;

public interface IParkingSpotRepository
{
    Task<int> AvailableCountAsync(AircraftType type);
    Task<List<ParkingSpot>> GetAllAsync();
    Task<ParkingSpot?> GetFirstAvailableAsync(AircraftType type);
    Task SaveChangesAsync();
    Task<List<ParkingSpot>> GetAllAvailableAsync();
}
