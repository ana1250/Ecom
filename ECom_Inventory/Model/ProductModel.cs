using System.ComponentModel.DataAnnotations.Schema;

namespace ECom_Inventory.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price {  get; set; } 
        
        public int Stock { get; set; }

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>(); 
    }

    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();  
    }



}
