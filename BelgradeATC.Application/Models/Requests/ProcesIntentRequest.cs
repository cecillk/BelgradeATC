using System;
using BelgradeATC.Core.Enums;

namespace BelgradeATC.Application.Models.Requests;

public class ProcesIntentRequest
{
  public required string CallSign { get; set; }
  public AircraftState RequestedState { get; set; }

}
