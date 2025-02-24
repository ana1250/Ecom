using ECom_Inventory.Data;
using ECom_Inventory.Interfaces;
using ECom_Inventory.Model;
using Microsoft.EntityFrameworkCore;
using Serilog;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogActionAsync(string username, string email, string action, string ipAddress)
    {
        try
        {
            var log = new AuthLog
            {
                Username = username,
                Email = email,
                ActionType = action,
                ActionTimestamp = DateTime.UtcNow,
                IPAddress = ipAddress
            };

            _context.AuthLog.Add(log);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error logging action for user {Username}", username);
        }
    }

    public async Task<DateTime?> GetLastLoginAsync(string username)
    {
        try
        {
            return await _context.AuthLog
                .Where(log => log.Username == username && log.ActionType == "Login")
                .OrderByDescending(log => log.ActionTimestamp)
                .Select(log => (DateTime?)log.ActionTimestamp)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving last login for user {Username}", username);
            return null;
        }
    }

    public async Task<User> GetProfileAsync(string username)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving profile for user {Username}", username);
            return null;
        }
    }

    public async Task<bool> UpdateUserAsync(string username, string email, string? password = null, string? role = null)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return false;

            bool isUpdated = false;

            if (user.Email != email)
            {
                user.Email = email;
                _context.Entry(user).Property(u => u.Email).IsModified = true;
                isUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                _context.Entry(user).Property(u => u.PasswordHash).IsModified = true;
                isUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(role) && user.Role != role)
            {
                user.Role = role;
                _context.Entry(user).Property(u => u.Role).IsModified = true;
                isUpdated = true;
            }

            if (isUpdated)
            {
                await _context.SaveChangesAsync();
            }

            return isUpdated;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating user {Username}", username);
            return false;
        }
    }

    public async Task<User> LoginAsync(string username, string password)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                Log.Warning("User {Username} not found during login.", username);
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (isPasswordValid)
            {
                return user;
            }

            Log.Warning("Invalid password for user {Username}.", username);
            return null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during login for user {Username}", username);
            return null;
        }
    }

    public async Task<(bool Success, string Message)> RegisterUserAsync(AddUserRequestModel newUser)
    {
        try
        {
            if (await _context.Users.AnyAsync(u => u.Username == newUser.Username))
                return (false, "Username is already taken.");

            if (await _context.Users.AnyAsync(u => u.Email == newUser.Email))
                return (false, "Email is already in use.");

            var user = new User
            {
                Username = newUser.Username,
                Email = newUser.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password),
                Role = newUser.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            string ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
            await LogActionAsync(newUser.Username, newUser.Email, "Register", ipAddress);

            return (true, "User registered successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error registering user {Username}", newUser.Username);
            return (false, "An error occurred while creating the account.");
        }
    }
}