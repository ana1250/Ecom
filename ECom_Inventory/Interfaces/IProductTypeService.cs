using ECom_Inventory.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECom_Inventory.Interfaces
{
    public interface IProductTypeService
    {
        Task<List<ProductType>> GetAllProductTypesAsync();
        Task<ProductType> GetProductTypeByIdAsync(int id);
        Task AddProductTypeAsync(ProductType productType);
        Task UpdateProductTypeAsync(ProductType productType);
        Task DeleteProductTypeAsync(int id);
        Task<int> GetProductTypesCountAsync();
    }
}