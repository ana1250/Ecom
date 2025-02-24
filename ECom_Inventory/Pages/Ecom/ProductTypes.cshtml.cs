using ECom_Inventory.Interfaces;
using ECom_Inventory.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECom_Inventory.Pages.Ecom
{
    [Authorize]
    public class ProductTypesModel : PageModel
    {
        private readonly IProductTypeService _productTypeService;
        private readonly ILogger<ProductTypesModel> _logger;

        public ProductTypesModel(IProductTypeService productTypeService, ILogger<ProductTypesModel> logger)
        {
            _productTypeService = productTypeService;
            _logger = logger;
        }

        public List<ProductType> ProductTypes { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                ProductTypes = await _productTypeService.GetAllProductTypesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product types.");
                ModelState.AddModelError(string.Empty, "An error occurred while fetching product types.");
            }
        }

        public async Task<JsonResult> OnPostCreateAsync([FromBody] ProductType productType)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, message = "Invalid data." });

                await _productTypeService.AddProductTypeAsync(productType);
                return new JsonResult(new { success = true, message = "Product type added successfully.", productType });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product type.");
                return new JsonResult(new { success = false, message = "An error occurred while creating the product type." });
            }
        }

        public async Task<JsonResult> OnPostEditAsync([FromBody] ProductType productType)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, message = "Invalid data." });

                await _productTypeService.UpdateProductTypeAsync(productType);
                return new JsonResult(new { success = true, message = "Product type updated successfully.", productType });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product type.");
                return new JsonResult(new { success = false, message = "An error occurred while updating the product type." });
            }
        }

        public async Task<JsonResult> OnPostDeleteAsync([FromBody] int id)
        {
            try
            {
                await _productTypeService.DeleteProductTypeAsync(id);
                return new JsonResult(new { success = true, message = "Product type deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product type.");
                return new JsonResult(new { success = false, message = "An error occurred while deleting the product type." });
            }
        }
    }
}