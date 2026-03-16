using ECom_Inventory.Data;
using ECom_Inventory.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using ECom_Inventory.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ECom_Inventory.Pages.Auth
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
		private readonly IAuthService _authService;
		private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LoginModel> _logger;
        private readonly ITokenService _tokenService;

		public LoginModel(AppDbContext context, IConfiguration configuration,IAuthService authService ,IHttpContextAccessor httpContextAccessor,ILogger<LoginModel> logger,ITokenService tokenService)
        {
            _context = context;
            _configuration = configuration;
			_authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _tokenService = tokenService;

		}

        [BindProperty]
        public LoginInputModel Input { get; set; } = new LoginInputModel();

        public string? ErrorMessage { get; set; }

		public async Task<IActionResult> OnPostAsync()
		{
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _authService.LoginAsync(Input.Username, Input.Password);

    		if (user == null)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }
            var token = _tokenService.GenerateToken(user);

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Token generation failed for user {Username}.", Input.Username);
                ErrorMessage = "An error occurred during login. Please try again.";
                return Page();
            }

            Response.Cookies.Append("AuthToken", token, new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                Expires = Input.RememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddHours(1)
            });

            string ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
            await _authService.LogActionAsync(Input.Username, user.Email, "Login", ipAddress);
            return RedirectToPage("/Index");
        }

    }
}
