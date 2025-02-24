using ECom_Inventory.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECom_Inventory.Interfaces
{
    public interface ISupplierService
    {
        Task<List<Supplier>> GetAllSuppliersAsync();
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task AddSupplierAsync(Supplier supplier);
        Task UpdateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(int id);
        Task<int> GetSuppliersCountAsync();
    }
}