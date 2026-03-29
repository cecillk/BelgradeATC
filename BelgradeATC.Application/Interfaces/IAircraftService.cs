using BelgradeATC.Application.Models;
using BelgradeATC.Application.Models.Requests;
using BelgradeATC.Application.Models.Responses;
using BelgradeATC.Core.Enums;

namespace BelgradeATC.Application.Interfaces;

public interface IAircraftService
{
    Task<bool> UpdateLocationAsync(UpdateLocationRequest request);
    Task<ProcesIntentResponse> ProcessIntentAsync(ProcesIntentRequest request);
}
