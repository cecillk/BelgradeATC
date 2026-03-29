using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Interfaces.Repositories;
using BelgradeATC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BelgradeATC.Infrastructure.Repositories;

public class StateChangeLogRepository : IStateChangeLogRepository
{
    private readonly AppDbContext _context;

    public StateChangeLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(StateChangeLog log)
        => await _context.StateChangeLogs.AddAsync(log);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task<List<StateChangeLog>> GetRecentAsync(int count)
        => await _context.StateChangeLogs
            .Include(l => l.Aircraft)
            .OrderByDescending(l => l.Timestamp)
            .Take(count)
            .ToListAsync();

    public async Task<List<StateChangeLog>> GetByAircraftAsync(string callSign)
        => await _context.StateChangeLogs
            .Include(l => l.Aircraft)
            .Where(l => l.Aircraft.CallSign == callSign)
            .OrderByDescending(l => l.Timestamp)
            .ToListAsync();

    public async Task<List<StateChangeLog>> GetAllAsync()
      => await _context.StateChangeLogs
              .Include(l => l.Aircraft)
              .OrderByDescending(l => l.Timestamp)
              .ToListAsync();
}
