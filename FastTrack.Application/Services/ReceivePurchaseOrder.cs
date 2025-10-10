using FastTrack.Core.Entities;
using FastTrack.Core.Enums;
using FastTrack.Persistence.Models;
using FastTrack.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Shared.Common.Result;

namespace FastTrack.Application.Services;

public class ReceivePurchaseOrder(
    IRepository<PurchaseOrderModel, PurchaseOrder> purchaseOrders,
    IRepository<InventoryMovementModel, InventoryMovement> movements,
    IRepository<CurrentInventoryModel, CurrentInventory> inventories,
    IUnitOfWork uow)
{
    private static readonly AsyncRetryPolicy RetryPolicy = Policy
        .Handle<DbUpdateConcurrencyException>()
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt) + Random.Shared.Next(0, 50)),
            onRetry: (ex, delay, attempt, ctx) =>
            {
                Console.WriteLine(
                    $"[Resiliencia] Reintento {attempt} tras conflicto de concurrencia ({ex.Message}). Esperando {delay.TotalMilliseconds}ms.");
            });
    
    public async Task<Result<string, string>> ReceiveAsync(int orderId, int updatedBy, CancellationToken ct = default)
    {
        PurchaseOrder? order = await purchaseOrders.GetByIdAsync(
            orderId,
            include: q => q.Include(o => o.OrderDetails),
            isTracking: true,
            cancellationToken: ct
        );

        if (order is null)
            return Result<string, string>.SetError("Orden de compra no encontrada.");

        if (order.IsReceived())
            return Result<string, string>.SetError("La orden ya fue recibida previamente.");

        order.MarkAsReceived(updatedBy);

        await uow.BeginTransactionAsync(ct);
        try
        {
            foreach (OrderDetail detail in order.OrderDetails)
            {
                InventoryMovement movement = InventoryMovement
                    .Create(detail.SkuCode, detail.Quantity, InventoryMovementType.In, order.KioskId, detail.ProductId,
                        order.Id, order.AddedBy);

                await movements.AddAsync(movement, ct);

                CurrentInventory? inventory = (await inventories.FindAsync(
                    i => i.ProductId == detail.ProductId && i.KioskId == order.KioskId,
                    isTracking: true,
                    cancellationToken: ct
                )).FirstOrDefault();

                if (inventory is null)
                {
                    inventory = CurrentInventory.Create(order.KioskId, detail.ProductId, detail.Quantity, detail.UpdatedBy);
                    await inventories.AddAsync(inventory, ct);
                }
                else
                {
                    inventory.AddQuantity(detail.Quantity, detail.UpdatedBy);
                    inventories.Update(inventory);
                }
            }

            purchaseOrders.Update(order, ct);
            await uow.SaveChangesAsync(ct);
            await uow.CommitAsync(ct);
            return Result<string, string>.SetSuccess("Orden recibida correctamente.");
        }
        catch (DbUpdateConcurrencyException)
        {
            await uow.RollbackAsync(ct);
            throw;
        }
        catch(Exception ex)
        {
            await uow.RollbackAsync(ct);
            return Result<string, string>.SetError($"Error al recibir orden: {ex.Message}");
        }
    }


}