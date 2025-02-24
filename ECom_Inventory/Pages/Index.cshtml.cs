using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace ECom_Inventory.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }

            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null || jwtToken.ValidTo < System.DateTime.UtcNow)
                {
                    return RedirectToPage("/Auth/Login");
                }
            }
            catch
            {
                return RedirectToPage("/Auth/Login");
            }

            return RedirectToPage("/Dashboard");
        }
    }
}
