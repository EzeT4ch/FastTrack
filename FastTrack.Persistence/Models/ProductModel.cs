using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastTrack.Persistence.Models;

[Table("Products")]
public class ProductModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int KioskId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [Required]
    public string Sku { get; set; }
    
    [Required]
    public int Status { get; set; }
    
    public DateTime DateAdded { get; set; } = DateTime.Now;
    
    public int AddedBy { get; set; }
    
    public DateTime LastUpdate { get; set; }
    
    public int UpdatedBy { get; set; }
    
    public virtual KioskModel Kiosk { get; set; }
}