using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BelgradeATC.API.Pages
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class LogsModel : PageModel
    {
        private readonly IStateChangeLogRepository _stateChangeLogRepository;

        public LogsModel(IStateChangeLogRepository stateChangeLogRepository)
        {
            _stateChangeLogRepository = stateChangeLogRepository;
        }

        public List<StateChangeLog> RecentLogs { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            RecentLogs = await _stateChangeLogRepository.GetAllAsync();
            return Page();
        }
    }
}
