using System.Reflection;
using FastTrack.Core.Entities;
using FastTrack.Persistence.Extensions;
using FastTrack.Persistence.Models;

namespace FastTrack.Persistence.Mappers;

public static class PurchaseOrderMapper
{
    public static PurchaseOrder ToDomain(this PurchaseOrderModel model)
    {
        PurchaseOrder entity = (PurchaseOrder)Activator.CreateInstance(
            typeof(PurchaseOrder),
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, null, null)!;

        entity.SetPrivateProperty(nameof(PurchaseOrder.Id), model.Id);
        entity.SetPrivateProperty(nameof(PurchaseOrder.ExternalOrderkey), model.ExternalOrderkey);
        entity.SetPrivateProperty(nameof(PurchaseOrder.TotalLines), model.TotalLines);
        entity.SetPrivateProperty(nameof(PurchaseOrder.TotalQuantity), model.TotalQuantity);
        entity.SetPrivateProperty(nameof(PurchaseOrder.Status), model.Status);
        entity.SetPrivateProperty(nameof(PurchaseOrder.KioskId), model.KioskId);
        entity.SetPrivateProperty(nameof(PurchaseOrder.DateAdded), model.DateAdded);
        entity.SetPrivateProperty(nameof(PurchaseOrder.AddedBy), model.AddedBy);
        entity.SetPrivateProperty(nameof(PurchaseOrder.LastUpdate), model.LastUpdate);
        entity.SetPrivateProperty(nameof(PurchaseOrder.UpdatedBy), model.UpdatedBy);

        return entity;
    }
    
    public static PurchaseOrderModel ToModel(this PurchaseOrder entity)
    {
        return new PurchaseOrderModel()
        {
            Id = entity.Id,
            ExternalOrderkey = entity.ExternalOrderkey,
            TotalLines = entity.TotalLines,
            TotalQuantity = entity.TotalQuantity,
            Status = entity.Status,
            KioskId = entity.KioskId,
            DateAdded = entity.DateAdded,
            AddedBy = entity.AddedBy,
            LastUpdate = entity.LastUpdate,
            UpdatedBy = entity.UpdatedBy
        };
    }
}