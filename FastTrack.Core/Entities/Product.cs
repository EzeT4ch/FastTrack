using FastTrack.Core.Exceptions;
using Shared.Abstractions.Enums;
using Shared.Abstractions.Interfaces;

namespace FastTrack.Core.Entities;

public class Product : IEntity, ICreatedAuditable, IUpdateAuditable
{
    private Product()
    {
    }

    private Product(int kioskId, string skuCode, SimpleStatus status, int userId)
    {
        KioskId = kioskId;
        SkuCode = skuCode;
        Status = status;
        DateAdded = DateTime.UtcNow;
        AddedBy = userId;
        LastUpdate = DateAdded;
        UpdatedBy = userId;
    }

    public int KioskId { get; private set; }

    public string Name { get; private set; }

    public string SkuCode { get; private set; }

    public SimpleStatus Status { get; private set; }

    public DateTime DateAdded { get; }

    public int AddedBy { get; }
    public int Id { get; private set; }

    public DateTime LastUpdate { get; }

    public int UpdatedBy { get; }

    public static Product Create(int kioskId, string skuCode, SimpleStatus status, int userId)
    {
        if (kioskId <= 0)
        {
            throw new DomainException("Debe especificarse un kiosco válido.", nameof(kioskId));
        }

        if (string.IsNullOrWhiteSpace(skuCode))
        {
            throw new DomainException("El código SKU no puede estar vacío.", nameof(skuCode));
        }

        if (!Enum.IsDefined(typeof(SimpleStatus), status))
        {
            throw new DomainException("El estado no es válido.", nameof(status));
        }

        if (userId <= 0)
        {
            throw new DomainException("El usuario creador debe ser válido.", nameof(userId));
        }

        return new Product(kioskId, skuCode.Trim(), status, userId);
    }
}