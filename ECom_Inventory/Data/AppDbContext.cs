using ECom_Inventory.Model;
using Microsoft.EntityFrameworkCore;


namespace ECom_Inventory.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AuthLog> AuthLog { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<AuditLogModel> ProductAuditLog { get; set; }
    }
}
