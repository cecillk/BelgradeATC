using BelgradeATC.Core.Entities;
using MediatR;

namespace BelgradeATC.Application.Queries;

public class GetAllAircraftQuery : IRequest<List<Aircraft>>
{
}
