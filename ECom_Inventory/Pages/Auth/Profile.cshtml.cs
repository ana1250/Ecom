using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ECom_Inventory.Model;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ECom_Inventory.Interfaces;
namespace ECom_Inventory.Pages.Auth;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly IAuthService _authService;
    private readonly ILogger<ProfileModel> _logger;

    public ProfileModel(IAuthService authService, ILogger<ProfileModel> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public string UserName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string LastLogin { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {

        UserName = User.FindFirstValue("name") ?? "N/A";
        var user = await _authService.GetProfileAsync(UserName);
        UserName = user.Username ?? "N/A";
        Email = user.Email ?? "N/A";
        Role = user.Role ?? "N/A";

        var lastLoginTime = await _authService.GetLastLoginAsync(UserName);
        LastLogin = lastLoginTime.HasValue ? lastLoginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "No login record found";

        return Page();
    }
    public async Task<IActionResult> OnPostUpdateEmailAsync([FromBody] User updatedMail)
    {
        if (string.IsNullOrEmpty(updatedMail.Email))
            return BadRequest(new { success = false, message = "Email cannot be empty" });

        UserName = User.FindFirstValue("name") ?? "N/A";
        var user = await _authService.GetProfileAsync(UserName);
        if (user == null) return NotFound();
        bool isUpdated = await _authService.UpdateUserAsync(user.Username, updatedMail.Email);

        if (!isUpdated)
            return StatusCode(500, new { success = false, message = "Failed to update email" });

        return new JsonResult(new { success = true, message = "Email updated successfully" });
    }
}

