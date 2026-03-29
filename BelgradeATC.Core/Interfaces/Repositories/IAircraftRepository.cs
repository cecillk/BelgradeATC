using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Enums;

namespace BelgradeATC.Core.Interfaces.Repositories;

public interface IAircraftRepository
{
    Task<Aircraft?> GetByCallSignAsync(string callSign);
    Task AddAsync(Aircraft aircraft);
    Task SaveChangesAsync();
    Task<bool> AnyInStateAsync(AircraftState state);
    Task<List<Aircraft>> GetAllAsync();
    Task<List<Aircraft>> GetAllInStateAsync(AircraftState state);
}
