using System;
using System.Linq;
using ECom_Inventory.Data;
using ECom_Inventory.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using ECom_Inventory.Interfaces;

namespace ECom_Inventory.Pages.Auth
{
    [Authorize(Policy = "AdminOnly")]
    public class AddUserModel : PageModel
    {
		private readonly AppDbContext _context;
		private readonly IAuthService _authService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AddUserModel(AppDbContext context, IAuthService authService, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_authService = authService;
			_httpContextAccessor = httpContextAccessor;

		}

		[BindProperty]
        public AddUserRequestModel Input { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newUser = new AddUserRequestModel
            {
                Username = Input.Username,
                Email = Input.Email,
                Password = Input.Password,
                Role = Input.Role,
            };

            var (success, message) = await _authService.RegisterUserAsync(newUser);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

			string ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
			await _authService.LogActionAsync(Input.Username,Input.Email, "Adduser", ipAddress);
			return RedirectToPage("/Auth/Login");
            
        }
    }
}
