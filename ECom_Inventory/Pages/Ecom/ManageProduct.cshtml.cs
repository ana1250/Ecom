using ECom_Inventory.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using ECom_Inventory.Interfaces;

namespace ECom_Inventory.Pages.Ecom
{
    [Authorize]
    public class ManageProductsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IAuditLogService _auditLogService;

        public ManageProductsModel(IProductService productService, IAuditLogService auditLogService)
        {
            _productService = productService;
            _auditLogService = auditLogService;
        }

        public List<Product> Products { get; set; } = new();
        public List<Brand> Brands { get; set; } = new();
        public List<ProductType> ProductTypes { get; set; } = new();
        public List<Supplier> Suppliers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Products = await _productService.GetAllProductsAsync();
                Brands = await _productService.GetAllBrandsAsync();
                Suppliers = await _productService.GetAllSuppliersAsync();
                ProductTypes = await _productService.GetAllProductTypesAsync();
                return Page();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error loading products, brands, suppliers, or product types.");
                return RedirectToPage("/Error"); // Redirect to an error page
            }
        }
        [Authorize(Policy = "AdminOnly")]
        public async Task<JsonResult> OnPostCreateAsync([FromBody] Product product)
        {
            try
            {
                product.CreatedAt = DateTime.UtcNow;
                product.CreatedBy = User.FindFirstValue("name") ?? "System";
                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = User.FindFirstValue("name") ?? "System";

                await _productService.AddProductAsync(product);

                // Log the creation of the product
                await _auditLogService.LogChangeAsync(
                    tableName: "Products",
                    recordId: product.Id,
                    action: "Create",
                    changedBy: User.FindFirstValue("name") ?? "System",
                    oldValues: null, // No old values for creation
                    newValues: product
                );

                return new JsonResult(new { success = true, message = "Product added successfully", product });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating product {ProductName}", product.Name);
                return new JsonResult(new { success = false, message = "An error occurred while creating the product." });
            }
        }

        public async Task<JsonResult> OnPostEditAsync([FromBody] Product product)
        {
            try
            {
                var existingProduct = await _productService.GetProductByIdAsync(product.Id);
                if (existingProduct == null)
                {
                    return new JsonResult(new { success = false, message = "Product not found." });
                }

                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = User.FindFirstValue("name") ?? "System";
                product.CreatedBy = existingProduct.CreatedBy;

                await _productService.UpdateProductAsync(product);

                // Log the update of the product
                await _auditLogService.LogChangeAsync(
                    tableName: "Products",
                    recordId: product.Id,
                    action: "Update",
                    changedBy: User.FindFirstValue("name") ?? "System",
                    oldValues: existingProduct, // Old values before update
                    newValues: product // New values after update
                );

                return new JsonResult(new { success = true, message = "Product updated successfully", product });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating product {ProductId}", product.Id);
                return new JsonResult(new { success = false, message = "An error occurred while updating the product." });
            }
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<JsonResult> OnPostDeleteAsync([FromBody] int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return new JsonResult(new { success = false, message = "Product not found." });
                }

                await _productService.DeleteProductAsync(id);

                // Log the deletion of the product
                await _auditLogService.LogChangeAsync(
                    tableName: "Products",
                    recordId: id,
                    action: "Delete",
                    changedBy: User.FindFirstValue("name") ?? "System",
                    oldValues: product, // Old values before deletion
                    newValues: null // No new values for deletion
                );

                return new JsonResult(new { success = true, message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting product {ProductId}", id);
                return new JsonResult(new { success = false, message = "An error occurred while deleting the product." });
            }
        }
    }
}