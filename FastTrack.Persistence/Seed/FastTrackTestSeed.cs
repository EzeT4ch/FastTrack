
﻿using FastTrack.Core.Enums;
using FastTrack.Persistence;

using FastTrack.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Enums;

public static class FastTrackDbSeeder
{
    public static async Task SeedAsync(FastTrackDbContext context)
    {
        if (await context.Kiosks.AnyAsync())
            return;

        Console.WriteLine("[FastTrack Seeder] Cargando datos iniciales...");

        // --- Kiosks ---
        var kiosks = Enumerable.Range(1, 5)
            .Select(i => new KioskModel
            {
                Code = $"KIOSK-{i:D2}",
                Name = $"Kiosk #{i}",

                Email = $"kiosk{i}@example.com",
                Address = $"Address {i}"
            }).ToArray();

        await context.Kiosks.AddRangeAsync(kiosks);

        await context.SaveChangesAsync(); // obtener Ids generados por la BD

        // --- Products ---
        var products = Enumerable.Range(1, 10)
            .Select(i => new ProductModel
            {

                KioskId = kiosks[(i - 1) % kiosks.Length].Id,
                Sku = $"SKU-{i:D3}",
                Name = $"Product {i}",
                Status = 1
            }).ToArray();

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // --- Purchase Orders ---
        var purchaseOrders = Enumerable.Range(1, 20)
            .Select(i => new PurchaseOrderModel
            {
                KioskId = kiosks[(i - 1) % kiosks.Length].Id,

                ExternalOrderkey = $"PO-{i:D5}",
                Status = i <= 10 ? SimpleStatus.Pendiente : SimpleStatus.Recibido,
                DateAdded = DateTime.UtcNow.AddDays(-i),
                TotalLines = 0,
                TotalQuantity = 0
            }).ToArray();

        await context.PurchaseOrders.AddRangeAsync(purchaseOrders);
        await context.SaveChangesAsync();

        // --- Order Details ---

        List<OrderDetailModel> details = new List<OrderDetailModel>();
        foreach (PurchaseOrderModel po in purchaseOrders)
        {
            int items = (po.Id % 3) + 2;
            for (int j = 0; j < items; j++)
            {
                var product = products[((po.Id + j - 1) % products.Length)];
                int quantity = ((j + 1) % 5) + 1;

                details.Add(new OrderDetailModel
                {
                    PurchaseOrderId = po.Id,
                    ProductId = product.Id,
                    Line = j + 1,
                    Quantity = quantity,
                    SkuCode = product.Sku
                });

                po.TotalLines += 1;
                po.TotalQuantity += quantity;
            }
        }

        await context.OrderDetails.AddRangeAsync(details);

        context.PurchaseOrders.UpdateRange(purchaseOrders);
        await context.SaveChangesAsync();

        // --- Current Inventories ---
        List<CurrentInventoryModel> inventories = new List<CurrentInventoryModel>();
        foreach (var kiosk in kiosks)
        {
            foreach (var product in products)
            {
                inventories.Add(new CurrentInventoryModel
                {
                    KioskId = kiosk.Id,
                    ProductId = product.Id,
                    Quantity = (kiosk.Id * 5 + product.Id) % 30 + 5
                });
            }
        }
        await context.CurrentInventories.AddRangeAsync(inventories);
        await context.SaveChangesAsync();

        // --- Inventory Movements ---
        List<InventoryMovementModel> movements = (from po in purchaseOrders.Where(p => p.Status == SimpleStatus.Recibido)
                                                 let relatedDetails = details.Where(d => d.PurchaseOrderId == po.Id)
                                                 from d in relatedDetails
                                                 select new InventoryMovementModel
                                                 {
                                                     ProductId = d.ProductId,
                                                     KioskId = po.KioskId,
                                                     OrderId = po.Id,
                                                     DateAdded = po.DateAdded.AddHours(2),
                                                     Quantity = d.Quantity,
                                                     InventoryMovementType = (int)InventoryMovementType.In,
                                                     SkuCode = d.SkuCode
                                                 }).ToList();
        await context.InventoryMovements.AddRangeAsync(movements);

        await context.InventoryMovements.AddRangeAsync(movements);
        await context.SaveChangesAsync();

        Console.WriteLine("[FastTrack Seeder] Datos iniciales cargados correctamente ✅");
    }
}
