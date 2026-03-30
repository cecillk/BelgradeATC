using System;
using BelgradeATC.Core.Enums;

namespace BelgradeATC.Application.Models.Requests;

public class ProcessIntentRequest
{
  public required string CallSign { get; set; } = string.Empty; 
  public AircraftState RequestedState { get; set; }

}
