using System;
using BelgradeATC.Core.Entities;
using MediatR;

namespace BelgradeATC.Application.Queries;

public class GetRecentLogsQuery : IRequest<List<StateChangeLog>>
{

}
