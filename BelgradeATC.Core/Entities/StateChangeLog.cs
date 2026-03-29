using BelgradeATC.Core.Enums;

namespace BelgradeATC.Core.Entities;

public class StateChangeLog
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public Guid? AircraftId { get; set; }
  public Aircraft Aircraft { get; set; } = null!;
  public AircraftState RequestedState { get; set; }
  public LogOutcome Outcome { get; set; }
  public string? Reason { get; set; }
  public DateTime Timestamp { get; set; }
}

public enum LogOutcome
{
  Accepted,
  Rejected
}
