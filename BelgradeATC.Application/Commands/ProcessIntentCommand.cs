using System.Text.Json.Serialization;
using BelgradeATC.Core.Enums;
using MediatR;

namespace BelgradeATC.Application.Commands;

public class ProcessIntentCommand : IRequest<ProcessIntentResult>
{
    public string CallSign { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public AircraftState RequestedState { get; set; }
}

public class ProcessIntentResult
{
    public bool Success { get; set; }
    public string? Reason { get; set; }
}
