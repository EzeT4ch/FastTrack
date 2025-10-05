using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastTrack.Persistence.Models;

[Table("OrderDetails")]
public class OrderDetailModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public int Line { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string SkuCode { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    [ForeignKey("PurchaseOrderId")]
    public int PurchaseOrderId { get; set; }
    
    [ForeignKey("ProductId")]
    public int ProductId { get; set; }
    
    public DateTime DateAdded { get; set; } = DateTime.Now;
    
    public int AddedBy { get; set; }
    
    public DateTime LastUpdate { get; set; }
    
    public int UpdatedBy { get; set; }
    
    public virtual PurchaseOrderModel PurchaseOrder { get; set; }
    
    public virtual ProductModel Product { get; set; }
}