using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastTrack.Persistence.Models;

[Table("InventoryMovements")]
public class InventoryMovementModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string SkuCode { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public int InventoryMovementType { get; set; }
    
    [Required]
    [ForeignKey("KioskId")]
    public int KioskId { get; set; }
    
    [Required]
    [ForeignKey("ProductId")]
    public int ProductId { get; set; }
    
    public DateTime DateAdded { get; set; } = DateTime.Now;
    
    public int AddedBy { get; set; }
    
    public DateTime LastUpdate { get; set; }
    
    public int UpdatedBy { get; set; }
    
    public virtual KioskModel Kiosk { get; set; }
    
    public virtual ProductModel Product { get; set; }
}