using ECom_Inventory.Model;

namespace ECom_Inventory.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
