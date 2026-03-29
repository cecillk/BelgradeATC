using BelgradeATC.Core.Entities;

namespace BelgradeATC.Core.Interfaces.Repositories;

public interface IStateChangeLogRepository
{
    Task AddAsync(StateChangeLog log);
    Task SaveChangesAsync();
    Task<List<StateChangeLog>> GetRecentAsync(int count);
    Task<List<StateChangeLog>> GetByAircraftAsync(string callSign);
    Task<List<StateChangeLog>> GetAllAsync();
}
