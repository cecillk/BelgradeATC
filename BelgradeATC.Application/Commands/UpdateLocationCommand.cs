using BelgradeATC.Core.Enums;
using MediatR;

namespace BelgradeATC.Application.Commands;

public class UpdateLocationCommand : IRequest<bool>
{
    public string CallSign { get; set; } = string.Empty;
    public AircraftType Type { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public int Altitude { get; set; }
    public int Heading { get; set; }
}
