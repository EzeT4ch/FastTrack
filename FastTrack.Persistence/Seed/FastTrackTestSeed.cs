<<<<<<< Updated upstream
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastTrack.Core.Enums;
using Microsoft.EntityFrameworkCore;
=======
﻿using FastTrack.Core.Enums;
using FastTrack.Persistence;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
                Email = $"kiosk{i}@example.com",
                Address = $"Address {i}"
=======
                Email = $"kiosk{i}@fasttrack.com",
                Address = $"Calle {i * 100}"
>>>>>>> Stashed changes
            }).ToArray();

        await context.Kiosks.AddRangeAsync(kiosks);
<<<<<<< Updated upstream
        await context.SaveChangesAsync(); // obtener Ids generados por la BD
=======
        await context.SaveChangesAsync();
>>>>>>> Stashed changes

        // --- Products ---
        var products = Enumerable.Range(1, 10)
            .Select(i => new ProductModel
            {
<<<<<<< Updated upstream
                KioskId = kiosks[(i - 1) % kiosks.Length].Id,
                Sku = $"SKU-{i:D3}",
                Name = $"Product {i}",
                Status = 1
=======
                Sku = $"SKU-{i:D3}",
                Name = $"Product {i}",
                KioskId = kiosks[(i - 1) % kiosks.Length].Id
>>>>>>> Stashed changes
            }).ToArray();

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // --- Purchase Orders ---
        var purchaseOrders = Enumerable.Range(1, 20)
            .Select(i => new PurchaseOrderModel
            {
                KioskId = kiosks[(i - 1) % kiosks.Length].Id,
<<<<<<< Updated upstream
                ExternalOrderkey = $"PO-{i:D5}",
                Status = i <= 10 ? SimpleStatus.Pendiente : SimpleStatus.Recibido,
                DateAdded = DateTime.UtcNow.AddDays(-i),
                TotalLines = 0,
                TotalQuantity = 0
=======
                Status = i <= 10 ? SimpleStatus.Pendiente : SimpleStatus.Recibido,
                ExternalOrderkey = $"PO-{i:D4}",
                DateAdded = DateTime.UtcNow.AddDays(-i)
>>>>>>> Stashed changes
            }).ToArray();

        await context.PurchaseOrders.AddRangeAsync(purchaseOrders);
        await context.SaveChangesAsync();

        // --- Order Details ---
<<<<<<< Updated upstream
        List<OrderDetailModel> details = new List<OrderDetailModel>();
        foreach (PurchaseOrderModel po in purchaseOrders)
=======
        var details = new List<OrderDetailModel>();
        foreach (var po in purchaseOrders)
>>>>>>> Stashed changes
        {
            int items = (po.Id % 3) + 2;
            for (int j = 0; j < items; j++)
            {
<<<<<<< Updated upstream
                var product = products[((po.Id + j - 1) % products.Length)];
                int quantity = ((j + 1) % 5) + 1;
=======
                int productId = products[(po.Id + j) % products.Length].Id;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
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
=======
        await context.SaveChangesAsync();

        // --- Current Inventories ---
        var inventories = (from kiosk in kiosks
                           from product in products
                           select new CurrentInventoryModel
                           {
                               KioskId = kiosk.Id,
                               ProductId = product.Id,
                               Quantity = (kiosk.Id * 3 + product.Id) % 20 + 10
                           }).ToList();

>>>>>>> Stashed changes
        await context.CurrentInventories.AddRangeAsync(inventories);
        await context.SaveChangesAsync();

        // --- Inventory Movements ---
<<<<<<< Updated upstream
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
=======
        var movements = (from po in purchaseOrders.Where(p => p.Status == SimpleStatus.Recibido)
                         let relatedDetails = details.Where(d => d.PurchaseOrderId == po.Id)
                         from d in relatedDetails
                         select new InventoryMovementModel
                         {
                             ProductId = d.ProductId,
                             KioskId = po.KioskId,
                             DateAdded = po.DateAdded.AddHours(2),
                             Quantity = 5,
                             SkuCode = d.SkuCode,
                             AddedBy = 1,
                             InventoryMovementType = (int)InventoryMovementType.In,
                             OrderId = po.Id
                         }).ToList();
>>>>>>> Stashed changes

        await context.InventoryMovements.AddRangeAsync(movements);
        await context.SaveChangesAsync();

        Console.WriteLine("[FastTrack Seeder] Datos iniciales cargados correctamente ✅");
    }
}
