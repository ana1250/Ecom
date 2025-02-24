using System.ComponentModel.DataAnnotations;

namespace ECom_Inventory.Model;
public class Supplier
{
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    [Required]
    [StringLength(255)]
    public string Product { get; set; }

    [Required]
    [StringLength(20)]
    public string Phone { get; set; }
}
