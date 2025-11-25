using System.Reflection;
using FastTrack.Core.Entities;
using FastTrack.Persistence.Extensions;
using FastTrack.Persistence.Models;

namespace FastTrack.Persistence.Mappers;

public static class ProductMapper
{
    public static Product ToDomain(this ProductModel model)
    {
        Product entity = (Product)Activator.CreateInstance(
            typeof(Product),
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, null, null)!;

        entity.SetPrivateProperty(nameof(Product.Id), model.Id);
        entity.SetPrivateProperty(nameof(Product.SkuCode), model.Sku);
        entity.SetPrivateProperty(nameof(Product.Name), model.Name);
        entity.SetPrivateProperty(nameof(Product.Status), model.Status);
        entity.SetPrivateProperty(nameof(Product.DateAdded), model.DateAdded);
        entity.SetPrivateProperty(nameof(Product.AddedBy), model.AddedBy);
        entity.SetPrivateProperty(nameof(Product.LastUpdate), model.LastUpdate);
        entity.SetPrivateProperty(nameof(Product.UpdatedBy), model.UpdatedBy);
        entity.SetPrivateProperty(nameof(Product.Price), model.Price);
        entity.SetPrivateProperty(nameof(Product.Description), model.Description);

        return entity;
    }

    public static ProductModel ToModel(this Product entity)
    {
        return new ProductModel
        {
            Id = entity.Id,
            KioskId = entity.KioskId,
            Sku = entity.SkuCode,
            Name = entity.Name,
            Status = (int)entity.Status,
            DateAdded = entity.DateAdded,
            AddedBy = entity.AddedBy,
            LastUpdate = entity.LastUpdate,
            UpdatedBy = entity.UpdatedBy,
            Description = entity.Description,
            Price = entity.Price,
        };
    }
}