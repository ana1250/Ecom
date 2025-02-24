using ECom_Inventory.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ECom_Inventory.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly ILogger<DashboardModel> _logger;
        private readonly IBrandService _brandService;
        private readonly IProductTypeService _productTypeService;
        private readonly ISupplierService _supplierService;

        public DashboardModel(
            ILogger<DashboardModel> logger,
            IBrandService brandService,
            IProductTypeService productTypeService,
            ISupplierService supplierService)
        {
            _logger = logger;
            _brandService = brandService;
            _productTypeService = productTypeService;
            _supplierService = supplierService;
        }

        public int BrandsCount { get; set; }
        public int ProductTypesCount { get; set; }
        public int SuppliersCount { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                // Fetch counts using services
                BrandsCount = await _brandService.GetBrandsCountAsync();
                ProductTypesCount = await _productTypeService.GetProductTypesCountAsync();
                SuppliersCount = await _supplierService.GetSuppliersCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching dashboard data.");
                ModelState.AddModelError(string.Empty, "An error occurred while fetching dashboard data.");
            }
        }
    }
}