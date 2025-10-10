using FastTrack.Core.Enums;
using Microsoft.EntityFrameworkCore;
using FastTrack.Persistence.Models;
using Shared.Abstractions.Enums;

namespace FastTrack.Persistence.Seed;

public static class FastTrackDbSeeder
{
    public static async Task SeedAsync(FastTrackDbContext context)
    {
        // Si la base no tiene kioskos, asumimos que está vacía
        if (await context.Kiosks.AnyAsync())
            return;

        // --- Kiosks ---
        KioskModel[] kiosks = Enumerable.Range(1, 5)
            .Select(i => new KioskModel
            {
                Id = i,
                Code = $"KIOSK-{i:D2}",
                Name = $"Kiosk #{i}"
            }).ToArray();
        await context.Kiosks.AddRangeAsync(kiosks);

        // --- Products ---
        ProductModel[] products = Enumerable.Range(1, 10)
            .Select(i => new ProductModel
            {
                Id = i,
                Sku = $"SKU-{i:D3}",
                Name = $"Product {i}"
            }).ToArray();
        await context.Products.AddRangeAsync(products);

        // --- Purchase Orders ---
        PurchaseOrderModel[] purchaseOrders = Enumerable.Range(1, 20)
            .Select(i => new PurchaseOrderModel
            {
                Id = i,
                KioskId = (i % 5) + 1,
                Status = i <= 10 ? SimpleStatus.Pendiente : SimpleStatus.Recibido,
                DateAdded = DateTime.UtcNow.AddDays(-i)
            }).ToArray();
        await context.PurchaseOrders.AddRangeAsync(purchaseOrders);

        // --- Order Details ---
        List<OrderDetailModel> details = new List<OrderDetailModel>();
        int detailId = 1;
        foreach (PurchaseOrderModel po in purchaseOrders)
        {
            int items = po.Id % 3 + 2;
            for (int j = 0; j < items; j++)
            {
                int productId = ((po.Id + j) % 10) + 1;
                details.Add(new OrderDetailModel
                {
                    Id = detailId++,
                    PurchaseOrderId = po.Id,
                    ProductId = productId,
                    Line = j + 1,
                    SkuCode = $"SKU-{productId:D3}"
                });
            }
        }
        await context.OrderDetails.AddRangeAsync(details);

        // --- Current Inventories ---
        List<CurrentInventoryModel> inventories = [];
        int invId = 1;
        inventories.AddRange(from kiosk in kiosks from product in products select new CurrentInventoryModel { Id = invId++, KioskId = kiosk.Id, ProductId = product.Id, Quantity = (kiosk.Id * 5 + product.Id) % 30 + 5 });
        await context.CurrentInventories.AddRangeAsync(inventories);

        // --- Inventory Movements ---
        int movId = 1;
        List<InventoryMovementModel> movements = (from po in purchaseOrders.Where(p => p.Status == SimpleStatus.Recibido)
        let relatedDetails = details.Where(d => d.PurchaseOrderId == po.Id)
        from d in relatedDetails
        select new InventoryMovementModel
        {
            Id = movId++,
            ProductId = d.ProductId,
            KioskId = po.KioskId,
            DateAdded = po.DateAdded.AddHours(2),
            Quantity = 5,
            InventoryMovementType = (int)InventoryMovementType.In
        }).ToList();
        await context.InventoryMovements.AddRangeAsync(movements);

        await context.SaveChangesAsync();
    }
}
