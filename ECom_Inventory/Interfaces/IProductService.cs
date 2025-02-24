namespace ECom_Inventory.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECom_Inventory.Model;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task<List<Brand>> GetAllBrandsAsync();
    Task<List<Supplier>> GetAllSuppliersAsync();
    Task<List<ProductType>> GetAllProductTypesAsync();
}
