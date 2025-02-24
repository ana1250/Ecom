using ECom_Inventory.Model;

namespace ECom_Inventory.Interfaces
{
    public interface IAuthService
    {
        Task LogActionAsync(string username, string email, string action, string ipAddress);
        Task<DateTime?> GetLastLoginAsync(string username);
        Task<User> GetProfileAsync(string username);
        Task<bool> UpdateUserAsync(string username, string email, string? password = null, string? role = null);
        Task<(bool Success, string Message)> RegisterUserAsync(AddUserRequestModel newUser);
        Task<User> LoginAsync(string username, string password);


    }
}
