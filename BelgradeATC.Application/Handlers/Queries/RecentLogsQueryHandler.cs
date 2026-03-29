using System;
using BelgradeATC.Application.Queries;
using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Interfaces.Repositories;
using MediatR;

namespace BelgradeATC.Application.Handlers.Queries;

public class RecentLogsQueryHandler(IStateChangeLogRepository statechangeLogRepository) : IRequestHandler<GetRecentLogsQuery, List<StateChangeLog>>
{
  public async Task<List<StateChangeLog>> Handle(GetRecentLogsQuery request, CancellationToken cancellationToken)
  {
    return await statechangeLogRepository.GetRecentAsync(10);
  }
}
