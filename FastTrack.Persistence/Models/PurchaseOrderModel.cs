using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Abstractions.Enums;

namespace FastTrack.Persistence.Models;

[Table("PurchaseOrders")]
public class PurchaseOrderModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string ExternalOrderkey { get; private set; }
    
    public SimpleStatus Status { get; private set; }
    
    [Required]
    public int TotalLines { get; private set; }
        
    [Required]
    public int TotalQuantity { get; private set; }

    [ForeignKey("KioskId")]
    public int KioskId { get; private set; }
        
    public DateTime DateAdded { get; private set; } = DateTime.Now;
        
    public int AddedBy { get; private set; }
        
    public DateTime LastUpdate { get; private set; }
        
    public int UpdatedBy { get; private set; }

    public virtual KioskModel Kiosk { get; private set; }
}