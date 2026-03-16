using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECom_Inventory.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            // If the user has no auth cookie, redirect to login.
            // If they do, redirect to Dashboard — the [Authorize] attribute
            // on DashboardModel will properly validate the JWT signature and expiry.
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }

            return RedirectToPage("/Dashboard");
        }
    }
}
