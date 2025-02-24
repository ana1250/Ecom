using ECom_Inventory.Data;
using ECom_Inventory.Interfaces;
using ECom_Inventory.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECom_Inventory.Services
{
    public class BrandService : IBrandService
    {
        private readonly AppDbContext _context;

        public BrandService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task AddBrandAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBrandAsync(Brand brand)
        {
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBrandAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetBrandsCountAsync()
        {
            return await _context.Brands.CountAsync();
        }
    }
}