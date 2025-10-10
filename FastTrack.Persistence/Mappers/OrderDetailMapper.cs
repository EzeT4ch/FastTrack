using System.Reflection;
using FastTrack.Core.Entities;
using FastTrack.Persistence.Extensions;
using FastTrack.Persistence.Models;

namespace FastTrack.Persistence.Mappers;

public static class OrderDetailMapper
{
    public static OrderDetail ToDomain(this OrderDetailModel model)
    {
        OrderDetail entity = (OrderDetail)Activator.CreateInstance(
            typeof(OrderDetail),
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, null, null)!;


        entity.SetPrivateProperty(nameof(OrderDetail.Id), model.Id);
        entity.SetPrivateProperty(nameof(OrderDetail.PurchaseOrderId), model.PurchaseOrderId);
        entity.SetPrivateProperty(nameof(OrderDetail.ProductId), model.ProductId);
        entity.SetPrivateProperty(nameof(OrderDetail.SkuCode), model.SkuCode);
        entity.SetPrivateProperty(nameof(OrderDetail.Line), model.Line);
        entity.SetPrivateProperty(nameof(OrderDetail.DateAdded), model.DateAdded);
        entity.SetPrivateProperty(nameof(OrderDetail.AddedBy), model.AddedBy);
        entity.SetPrivateProperty(nameof(OrderDetail.LastUpdate), model.LastUpdate);
        entity.SetPrivateProperty(nameof(OrderDetail.UpdatedBy), model.UpdatedBy);

        return entity;
    }

    public static OrderDetailModel ToModel(this OrderDetail entity)
    {
        return new OrderDetailModel
        {
            Id = entity.Id,
            PurchaseOrderId = entity.PurchaseOrderId,
            ProductId = entity.ProductId,
            SkuCode = entity.SkuCode,
            Line = entity.Line,
            DateAdded = entity.DateAdded,
            AddedBy = entity.AddedBy,
            LastUpdate = entity.LastUpdate,
            UpdatedBy = entity.UpdatedBy
        };
    }
}