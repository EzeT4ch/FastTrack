using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastTrack.Persistence.Models;

[Table("CurrentInventories")]
public class CurrentInventoryModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    public int KioskId { get; set; }
    
    public DateTime DateAdded { get; set; } = DateTime.Now;
    
    public int AddedBy { get; set; }
    
    public DateTime LastUpdate { get; set; }
    
    public int UpdatedBy { get; set; }
    
    public virtual KioskModel Kiosk { get; set; }
    
    public virtual ICollection<ProductModel> Products { get; set; } = [];
}