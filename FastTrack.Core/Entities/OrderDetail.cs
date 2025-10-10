using FastTrack.Core.Exceptions;
using Shared.Abstractions.Interfaces;

namespace FastTrack.Core.Entities;

public class OrderDetail : IEntity, ICreatedAuditable, IUpdateAuditable
{
    private OrderDetail(int line, string skuCode, int purchaseOrderId, int productId, int userId, int quantity)
    {
        Line = line;
        SkuCode = skuCode;
        PurchaseOrderId = purchaseOrderId;
        ProductId = productId;
        Quantity = quantity;
        DateAdded = DateTime.UtcNow;
        AddedBy = userId;
        LastUpdate = DateAdded;
        UpdatedBy = userId;
    }

    public int Line { get; private set; }
    public string SkuCode { get; private set; }
    public int PurchaseOrderId { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; } = 1;
    public DateTime DateAdded { get; }
    public int AddedBy { get; }
    public int Id { get; private set; }
    public DateTime LastUpdate { get; }
    public int UpdatedBy { get; }

    public static OrderDetail Create(int line, string skuCode, int purchaseOrderId, int productId, int userId, int quantity = 1)
    {
        if (line <= 0)
        {
            throw new DomainException("La línea de orden debe ser mayor que cero.", nameof(line));
        }

        if (string.IsNullOrWhiteSpace(skuCode))
        {
            throw new DomainException("El código SKU no puede estar vacío.", nameof(skuCode));
        }

        if (purchaseOrderId <= 0)
        {
            throw new DomainException("Debe especificarse una orden de compra válida.", nameof(purchaseOrderId));
        }

        if (productId <= 0)
        {
            throw new DomainException("Debe especificarse un producto válido.", nameof(productId));
        }

        if (userId <= 0)
        {
            throw new DomainException("El usuario creador debe ser válido.", nameof(userId));
        }

        if (quantity <= 0)
        {
            throw new DomainException("La cantidad debe ser mayor que cero.", nameof(quantity));
        }
        
        return new OrderDetail(line, skuCode.Trim(), purchaseOrderId, productId, userId, quantity);
    }
}