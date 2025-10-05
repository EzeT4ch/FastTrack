using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastTrack.Persistence.Models;

[Table("Kiosks")]
public class KioskModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Address { get; set; }
    
    public DateTime DateAdded { get; set; } = DateTime.Now;
    
    public int AddedBy { get; set; }
    
    public DateTime LastUpdate { get; set; }
    
    public int UpdatedBy { get; set; }
    
    public virtual ICollection<CurrentInventoryModel> CurrentInventories { get; set; } = [];
    public virtual ICollection<ProductModel> Products { get; set; } = [];
    public virtual ICollection<PurchaseOrderModel> PurchaseOrders { get; set; } = [];
}