using ECom_Inventory.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECom_Inventory.Interfaces
{
    public interface IBrandService
    {
        Task<List<Brand>> GetAllBrandsAsync();
        Task<Brand> GetBrandByIdAsync(int id);
        Task AddBrandAsync(Brand brand);
        Task UpdateBrandAsync(Brand brand);
        Task DeleteBrandAsync(int id);
        Task<int> GetBrandsCountAsync();
    }
}