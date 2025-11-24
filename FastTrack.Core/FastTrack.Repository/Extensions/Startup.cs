using FastTrack.Core.Entities;
using FastTrack.Persistence.Mappers;
using FastTrack.Persistence.Models;
using FastTrack.Repository.Interfaces;
using FastTrack.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FastTrack.Repository.Extensions;

public static class Startup
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddRepository<CurrentInventoryModel, CurrentInventory>(CurrentInventoryMappers.ToDomain,
                CurrentInventoryMappers.ToModel)
            .AddRepository<InventoryMovementModel, InventoryMovement>(InventoryMovementMapper.ToDomain,
                InventoryMovementMapper.ToModel)
            .AddRepository<KioskModel, Kiosk>(KioskMapper.MapToDomain, KioskMapper.MapToModel)
            .AddRepository<OrderDetailModel, OrderDetail>(OrderDetailMapper.ToDomain, OrderDetailMapper.ToModel)
            .AddRepository<ProductModel, Product>(ProductMapper.ToDomain, ProductMapper.ToModel)
            .AddRepository<PurchaseOrderModel,
                PurchaseOrder>(PurchaseOrderMapper.ToDomain, PurchaseOrderMapper.ToModel);
    }
}