using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Enums;
using BelgradeATC.Core.Interfaces.Repositories;
using BelgradeATC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BelgradeATC.Infrastructure.Repositories;

public class ParkingSpotRepository : IParkingSpotRepository
{
    private readonly AppDbContext _context;

    public ParkingSpotRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AvailableCountAsync(AircraftType type)
        => await _context.ParkingSpots.CountAsync(s => s.Type == type && s.OccupiedBy == null);

    public async Task<List<ParkingSpot>> GetAllAsync()
        => await _context.ParkingSpots.ToListAsync();

    public async Task<ParkingSpot?> GetFirstAvailableAsync(AircraftType type)
        => await _context.ParkingSpots.FirstOrDefaultAsync(s => s.Type == type && s.OccupiedBy == null);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task<List<ParkingSpot>> GetAllAvailableAsync()
        => await _context.ParkingSpots.Where(x => x.OccupiedBy == null).ToListAsync();

}
