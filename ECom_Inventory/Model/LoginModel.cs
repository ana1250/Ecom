using System.ComponentModel.DataAnnotations;
namespace ECom_Inventory.Model;

public class LoginInputModel
{
	[Required(ErrorMessage = "Username is required.")]
	public string Username { get; set; }

	[Required(ErrorMessage = "Password is required.")]
	public string Password { get; set; }

	public bool RememberMe { get; set; }
}
