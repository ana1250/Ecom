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
    public class BrandsModel : PageModel
    {
        private readonly IBrandService _brandService;
        private readonly ILogger<BrandsModel> _logger;

        public BrandsModel(IBrandService brandService, ILogger<BrandsModel> logger)
        {
            _brandService = brandService;
            _logger = logger;
        }

        public List<Brand> Brands { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                Brands = await _brandService.GetAllBrandsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching brands.");
                ModelState.AddModelError(string.Empty, "An error occurred while fetching brands.");
            }
        }

        public async Task<JsonResult> OnPostCreateAsync([FromBody] Brand brand)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, message = "Invalid data." });

                await _brandService.AddBrandAsync(brand);
                return new JsonResult(new { success = true, message = "Brand added successfully.", brand });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating brand.");
                return new JsonResult(new { success = false, message = "An error occurred while creating the brand." });
            }
        }

        public async Task<JsonResult> OnPostEditAsync([FromBody] Brand brand)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, message = "Invalid data." });

                await _brandService.UpdateBrandAsync(brand);
                return new JsonResult(new { success = true, message = "Brand updated successfully.", brand });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating brand.");
                return new JsonResult(new { success = false, message = "An error occurred while updating the brand." });
            }
        }

        public async Task<JsonResult> OnPostDeleteAsync([FromBody] int id)
        {
            try
            {
                await _brandService.DeleteBrandAsync(id);
                return new JsonResult(new { success = true, message = "Brand deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting brand.");
                return new JsonResult(new { success = false, message = "An error occurred while deleting the brand." });
            }
        }
    }
}