using System.Reflection;
using FastTrack.Core.Entities;
using FastTrack.Persistence.Extensions;
using FastTrack.Persistence.Models;

namespace FastTrack.Persistence.Mappers;

public static class InventoryMovementMapper
{
    public static InventoryMovement ToDomain(this InventoryMovementModel model)
    {
        InventoryMovement entity = (InventoryMovement)Activator.CreateInstance(
            typeof(InventoryMovement),
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, null, null)!;

        entity.SetPrivateProperty(nameof(InventoryMovement.Id), model.Id);
        entity.SetPrivateProperty(nameof(InventoryMovement.SkuCode), model.SkuCode);
        entity.SetPrivateProperty(nameof(InventoryMovement.Quantity), model.Quantity);
        entity.SetPrivateProperty(nameof(InventoryMovement.Type), model.InventoryMovementType);
        entity.SetPrivateProperty(nameof(InventoryMovement.KioskId), model.KioskId);
        entity.SetPrivateProperty(nameof(InventoryMovement.ProductId), model.ProductId);
        entity.SetPrivateProperty(nameof(InventoryMovement.OrderId), model.OrderId);
        entity.SetPrivateProperty(nameof(InventoryMovement.DateAdded), model.DateAdded);
        entity.SetPrivateProperty(nameof(InventoryMovement.AddedBy), model.AddedBy);
        entity.SetPrivateProperty(nameof(InventoryMovement.LastUpdate), model.LastUpdate);
        entity.SetPrivateProperty(nameof(InventoryMovement.UpdatedBy), model.UpdatedBy);

        return entity;
    }

    public static InventoryMovementModel ToModel(this InventoryMovement entity)
        => new()
        {
            Id = entity.Id,
            SkuCode = entity.SkuCode,
            Quantity = entity.Quantity,
            InventoryMovementType = (int)entity.Type,
            KioskId = entity.KioskId,
            ProductId = entity.ProductId,
            OrderId = entity.OrderId,
            DateAdded = entity.DateAdded,
            AddedBy = entity.AddedBy,
            LastUpdate = entity.LastUpdate,
            UpdatedBy = entity.UpdatedBy
        };
}