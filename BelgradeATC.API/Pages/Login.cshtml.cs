using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BelgradeATC.API.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _config;

        public LoginModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var username = _config["AdminCredentials:Username"];
            var password = _config["AdminCredentials:Password"];

            if (Username == username && Password == password)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, Username) };
                var identity = new ClaimsIdentity(claims, "AdminCookie");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("AdminCookie", principal);
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return Page();
        }
    }
}
