using FastTrack.Core.Exceptions;
using Shared.Abstractions.Enums;
using Shared.Abstractions.Interfaces;

namespace FastTrack.Core.Entities;

public class PurchaseOrder : IEntity, ICreatedAuditable, IUpdateAuditable
{
    private PurchaseOrder(string externalOrderKey, SimpleStatus status, int totalLines, int totalQuantity, int kioskId,
        int userId)
    {
        ExternalOrderkey = externalOrderKey;
        Status = status;
        TotalLines = totalLines;
        TotalQuantity = totalQuantity;
        KioskId = kioskId;
        DateAdded = DateTime.UtcNow;
        AddedBy = userId;
        LastUpdate = DateAdded;
        UpdatedBy = userId;
    }

    public string ExternalOrderkey { get; private set; }

    public SimpleStatus Status { get; private set; }

    public int TotalLines { get; private set; }

    public int TotalQuantity { get; private set; }

    public int KioskId { get; private set; }

    public DateTime DateAdded { get; }

    public int AddedBy { get; }
    public int Id { get; private set; }

    public DateTime LastUpdate { get; private set; }

    public int UpdatedBy { get; private set; }
    
    public virtual ICollection<OrderDetail> OrderDetails { get; private set; } = [];

    public static PurchaseOrder Create(string externalOrderKey, SimpleStatus status, int totalLines, int totalQuantity,
        int kioskId, int userId)
    {
        if (string.IsNullOrWhiteSpace(externalOrderKey))
        {
            throw new DomainException("El código de orden externa no puede estar vacío.", nameof(externalOrderKey));
        }

        if (!Enum.IsDefined(typeof(SimpleStatus), status))
        {
            throw new DomainException("El estado no es válido.", nameof(status));
        }

        if (totalLines <= 0)
        {
            throw new DomainException("El total de líneas debe ser mayor que cero.", nameof(totalLines));
        }

        if (totalQuantity <= 0)
        {
            throw new DomainException("La cantidad total debe ser mayor que cero.", nameof(totalQuantity));
        }

        if (kioskId <= 0)
        {
            throw new DomainException("Debe especificarse un kiosco válido.", nameof(kioskId));
        }

        if (userId <= 0)
        {
            throw new DomainException("El usuario creador debe ser válido.", nameof(userId));
        }

        return new PurchaseOrder(externalOrderKey.Trim(), status, totalLines, totalQuantity, kioskId, userId);
    }

    public void AddDetail(OrderDetail orderDetail)
    {
        if(Status != SimpleStatus.Pendiente)
        {
            throw new DomainException("No se pueden agregar detalles a una orden que no está pendiente.", nameof(Status));
        }
        
        OrderDetails.Add(orderDetail);
    }
    
    public void MarkAsReceived(int userId)
    {
        if (Status != SimpleStatus.Pendiente)
        {
            throw new DomainException("Solo se pueden recibir órdenes que estén pendientes.", nameof(Status));
        }

        Status = SimpleStatus.Recibido;
        LastUpdate = DateTime.UtcNow;
        UpdatedBy = userId;
    }
    
    public bool IsReceived() => Status == SimpleStatus.Recibido;
}