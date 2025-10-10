using FastTrack.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace FastTrack.Persistence;

public class FastTrackDbContext : DbContext
{
    public FastTrackDbContext(DbContextOptions<FastTrackDbContext> options)
        : base(options)
    {
    }

    public DbSet<KioskModel> Kiosks { get; set; }
    public DbSet<ProductModel> Products { get; set; }
    public DbSet<PurchaseOrderModel> PurchaseOrders { get; set; }
    public DbSet<OrderDetailModel> OrderDetails { get; set; }
    public DbSet<InventoryMovementModel> InventoryMovements { get; set; }
    public DbSet<CurrentInventoryModel> CurrentInventories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<KioskModel>()
            .HasKey(k => k.Id);

        modelBuilder.Entity<KioskModel>()
            .HasMany(k => k.PurchaseOrders)
            .WithOne(po => po.Kiosk)
            .HasForeignKey(po => po.KioskId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<KioskModel>()
            .HasMany(k => k.InventoryMovements)
            .WithOne(im => im.Kiosk)
            .HasForeignKey(im => im.KioskId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PurchaseOrderModel>()
            .HasKey(po => po.Id);

        modelBuilder.Entity<PurchaseOrderModel>()
            .HasMany(po => po.OrderDetails)
            .WithOne(od => od.PurchaseOrder)
            .HasForeignKey(od => od.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderDetailModel>()
            .HasKey(od => od.Id);

        modelBuilder.Entity<OrderDetailModel>()
            .HasOne(od => od.Product)
            .WithMany()
            .HasForeignKey(od => od.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductModel>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<ProductModel>()
            .HasMany(p => p.InventoryMovements)
            .WithOne(im => im.Product)
            .HasForeignKey(im => im.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<InventoryMovementModel>()
            .HasKey(im => im.Id);

        modelBuilder.Entity<InventoryMovementModel>()
            .HasOne(im => im.Kiosk)
            .WithMany(k => k.InventoryMovements)
            .HasForeignKey(im => im.KioskId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InventoryMovementModel>()
            .HasOne(im => im.Product)
            .WithMany(p => p.InventoryMovements)
            .HasForeignKey(im => im.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CurrentInventoryModel>()
            .HasKey(ci => ci.Id);

        modelBuilder.Entity<CurrentInventoryModel>()
            .HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CurrentInventoryModel>()
            .HasOne(ci => ci.Kiosk)
            .WithMany()
            .HasForeignKey(ci => ci.KioskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}