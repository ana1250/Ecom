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
    public class SuppliersModel : PageModel
    {
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SuppliersModel> _logger;

        public SuppliersModel(ISupplierService supplierService, ILogger<SuppliersModel> logger)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        public List<Supplier> Suppliers { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                Suppliers = await _supplierService.GetAllSuppliersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching suppliers.");
                ModelState.AddModelError(string.Empty, "An error occurred while fetching suppliers.");
            }
        }
        public async Task<JsonResult> OnPostCreateAsync([FromBody] Supplier supplier)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, message = "Invalid data." });

                await _supplierService.AddSupplierAsync(supplier);
                return new JsonResult(new { success = true, message = "Supplier added successfully.", supplier });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating supplier.");
                return new JsonResult(new { success = false, message = "An error occurred while creating the supplier." });
            }
        }

        public async Task<JsonResult> OnPostEditAsync([FromBody] Supplier supplier)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, message = "Invalid data." });

                await _supplierService.UpdateSupplierAsync(supplier);
                return new JsonResult(new { success = true, message = "Supplier updated successfully.", supplier });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating supplier.");
                return new JsonResult(new { success = false, message = "An error occurred while updating the supplier." });
            }
        }

        public async Task<JsonResult> OnPostDeleteAsync([FromBody] int id)
        {
            try
            {
                await _supplierService.DeleteSupplierAsync(id);
                return new JsonResult(new { success = true, message = "Supplier deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting supplier.");
                return new JsonResult(new { success = false, message = "An error occurred while deleting the supplier." });
            }
        }
    }
}