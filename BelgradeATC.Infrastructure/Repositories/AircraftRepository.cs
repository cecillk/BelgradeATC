using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Enums;
using BelgradeATC.Core.Interfaces.Repositories;
using BelgradeATC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BelgradeATC.Infrastructure.Repositories;

public class AircraftRepository : IAircraftRepository
{
    private readonly AppDbContext _context;

    public AircraftRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Aircraft?> GetByCallSignAsync(string callSign)
    {
        return await _context.Aircraft.FirstOrDefaultAsync(a => a.CallSign == callSign);
    }

    public async Task AddAsync(Aircraft aircraft)
    {
        await _context.Aircraft.AddAsync(aircraft);
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task<bool> AnyInStateAsync(AircraftState state)
        => await _context.Aircraft.AnyAsync(a => a.State == state);

    public async Task<List<Aircraft>> GetAllAsync()
        => await _context.Aircraft.ToListAsync();

    public async Task<List<Aircraft>> GetAllInStateAsync(AircraftState state)
        => await _context.Aircraft.Where(a => a.State == state).ToListAsync();
}
