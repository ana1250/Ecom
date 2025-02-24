using System;
using ECom_Inventory.Data;
using ECom_Inventory.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using ECom_Inventory.Interfaces;

namespace ECom_Inventory.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving all products.");
                return new List<Product>();
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                return await _context.Products.FindAsync(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving product with ID {ProductId}", id);
                return null;
            }
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error adding product {ProductName}", product.Name);
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(product.Id);
                if (existingProduct != null)
                {
                    _context.Entry(existingProduct).CurrentValues.SetValues(product); // Update values
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating product {ProductId}", product.Id);
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting product with ID {ProductId}", id);
            }
        }

        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            try
            {
                return await _context.Brands.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving all brands.");
                return new List<Brand>();
            }
        }

        public async Task<List<ProductType>> GetAllProductTypesAsync()
        {
            try
            {
                return await _context.ProductTypes.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving all product types.");
                return new List<ProductType>();
            }
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            try
            {
                return await _context.Suppliers.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving all suppliers.");
                return new List<Supplier>();
            }
        }
    }
}