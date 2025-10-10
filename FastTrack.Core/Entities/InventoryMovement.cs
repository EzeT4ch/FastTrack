using FastTrack.Core.Enums;
using FastTrack.Core.Exceptions;
using Shared.Abstractions.Interfaces;

namespace FastTrack.Core.Entities;

public class InventoryMovement : IEntity, ICreatedAuditable, IUpdateAuditable
{
    private InventoryMovement()
    {
    }

    private InventoryMovement(string skuCode, int quantity, InventoryMovementType type, int kioskId, int productId,
        int orderId, int userId)
    {
        SkuCode = skuCode;
        Quantity = quantity;
        Type = type;
        KioskId = kioskId;
        ProductId = productId;
        OrderId = orderId;
        DateAdded = DateTime.UtcNow;
        AddedBy = userId;
        LastUpdate = DateAdded;
        UpdatedBy = userId;
    }

    public string SkuCode { get; private set; }

    public int Quantity { get; private set; }

    public InventoryMovementType Type { get; private set; }

    public int KioskId { get; private set; }

    public int ProductId { get; private set; }

    public int OrderId { get; private set; }

    public DateTime DateAdded { get; }

    public int AddedBy { get; }
    public int Id { get; private set; }

    public DateTime LastUpdate { get; }

    public int UpdatedBy { get; }

    public static InventoryMovement Create(string skuCode, int quantity, InventoryMovementType type, int kioskId,
        int productId, int orderId, int userId)
    {
        if (string.IsNullOrWhiteSpace(skuCode))
        {
            throw new DomainException("El código SKU no puede estar vacío.", nameof(skuCode));
        }

        if (quantity <= 0)
        {
            throw new DomainException("La cantidad debe ser mayor que cero.", nameof(quantity));
        }

        if (!Enum.IsDefined(typeof(InventoryMovementType), type))
        {
            throw new DomainException("El tipo de movimiento no es válido.", nameof(type));
        }

        if (kioskId <= 0)
        {
            throw new DomainException("Debe especificarse un kiosco válido.", nameof(kioskId));
        }

        if (productId <= 0)
        {
            throw new DomainException("Debe especificarse un producto válido.", nameof(productId));
        }

        if (orderId <= 0)
        {
            throw new DomainException("Debe especificarse una orden válida.", nameof(orderId));
        }

        if (userId <= 0)
        {
            throw new DomainException("El usuario creador debe ser válido.", nameof(userId));
        }

        return new InventoryMovement(skuCode.Trim(), quantity, type, kioskId, productId, orderId, userId);
    }
}