using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BelgradeATC.API.Pages
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class AircraftModel : PageModel
    {
        private readonly IAircraftRepository repository;

        public AircraftModel(IAircraftRepository _repository)
        {
            repository = _repository;
        }
        public List<Aircraft> Aircrafts { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Aircrafts = await repository.GetAllAsync();

            return Page();
        }
    }
}
