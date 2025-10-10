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

    [Required] [MaxLength(50)] public string ExternalOrderkey { get;  set; }

    public SimpleStatus Status { get;  set; }

    [Required] public int TotalLines { get;  set; }

    [Required] public int TotalQuantity { get;  set; }

    [Required] public int KioskId { get;  set; }

    public DateTime DateAdded { get;  set; } = DateTime.Now;

    public int AddedBy { get;  set; }

    public DateTime LastUpdate { get;  set; }

    public int UpdatedBy { get;  set; }

    [ForeignKey(nameof(KioskId))] public virtual KioskModel Kiosk { get;  set; }

    public virtual ICollection<OrderDetailModel> OrderDetails { get;  set; } = [];
}