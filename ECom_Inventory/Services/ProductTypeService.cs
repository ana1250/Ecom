using ECom_Inventory.Data;
using ECom_Inventory.Interfaces;
using ECom_Inventory.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECom_Inventory.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly AppDbContext _context;

        public ProductTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductType>> GetAllProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }

        public async Task<ProductType> GetProductTypeByIdAsync(int id)
        {
            return await _context.ProductTypes.FindAsync(id);
        }

        public async Task AddProductTypeAsync(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductTypeAsync(ProductType productType)
        {
            _context.ProductTypes.Update(productType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductTypeAsync(int id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType != null)
            {
                _context.ProductTypes.Remove(productType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetProductTypesCountAsync()
        {
            return await _context.ProductTypes.CountAsync();
        }
    }
}