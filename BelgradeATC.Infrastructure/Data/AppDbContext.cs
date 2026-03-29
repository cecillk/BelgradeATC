using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace BelgradeATC.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Aircraft> Aircraft { get; set; }
    public DbSet<ParkingSpot> ParkingSpots { get; set; }
    public DbSet<StateChangeLog> StateChangeLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // CallSign must be unique — two aircraft cannot share the same identifier
        modelBuilder.Entity<Aircraft>()
            .HasIndex(a => a.CallSign)
            .IsUnique();

        // Each log entry belongs to one aircraft
        modelBuilder.Entity<StateChangeLog>()
       .HasOne(l => l.Aircraft)
       .WithMany()
       .HasForeignKey(l => l.AircraftId);

    }
}
