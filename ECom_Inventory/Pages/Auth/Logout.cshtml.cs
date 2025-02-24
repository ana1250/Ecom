using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECom_Inventory.Pages.Auth
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            Response.Cookies.Delete("AuthToken"); 
            return RedirectToPage("/Auth/Login"); 
        }
        public IActionResult OnPost()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToPage("/Auth/Login");
        }
    }
}
