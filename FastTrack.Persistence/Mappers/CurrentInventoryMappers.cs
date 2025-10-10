using System.Reflection;
using FastTrack.Core.Entities;
using FastTrack.Persistence.Extensions;
using FastTrack.Persistence.Models;

namespace FastTrack.Persistence.Mappers;

public static class CurrentInventoryMappers
{
    public static CurrentInventory ToDomain(this CurrentInventoryModel model)
    {
        CurrentInventory entity = (CurrentInventory)Activator.CreateInstance(
            typeof(CurrentInventory),
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, null, null)!;

        entity.SetPrivateProperty(nameof(CurrentInventory.Id), model.Id);
        entity.SetPrivateProperty(nameof(CurrentInventory.ProductId), model.ProductId);
        entity.SetPrivateProperty(nameof(CurrentInventory.Quantity), model.Quantity);
        entity.SetPrivateProperty(nameof(CurrentInventory.KioskId), model.KioskId);
        entity.SetPrivateProperty(nameof(CurrentInventory.DateAdded), model.DateAdded);
        entity.SetPrivateProperty(nameof(CurrentInventory.AddedBy), model.AddedBy);
        entity.SetPrivateProperty(nameof(CurrentInventory.LastUpdate), model.LastUpdate);
        entity.SetPrivateProperty(nameof(CurrentInventory.UpdatedBy), model.UpdatedBy);

        return entity;
    }

    public static CurrentInventoryModel ToModel(this CurrentInventory entity)
    {
        return new CurrentInventoryModel
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            KioskId = entity.KioskId,
            ProductId = entity.ProductId,
            DateAdded = entity.DateAdded,
            AddedBy = entity.AddedBy,
            LastUpdate = entity.LastUpdate,
            UpdatedBy = entity.UpdatedBy
        };
    }
}