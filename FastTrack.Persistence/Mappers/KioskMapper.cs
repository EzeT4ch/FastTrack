using System.Reflection;
using FastTrack.Core.Entities;
using FastTrack.Persistence.Extensions;
using FastTrack.Persistence.Models;

namespace FastTrack.Persistence.Mappers;

public static class KioskMapper
{
    public static Kiosk MapToDomain(this KioskModel entity)
    {
        Kiosk kiosk = (Kiosk)Activator.CreateInstance(
            typeof(Kiosk),
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, null, null)!;

        kiosk.SetPrivateProperty(nameof(Kiosk.Id), entity.Id);
        kiosk.SetPrivateProperty(nameof(Kiosk.Name), entity.Name);
        kiosk.SetPrivateProperty(nameof(Kiosk.Email), entity.Email);
        kiosk.SetPrivateProperty(nameof(Kiosk.Address), entity.Address);
        kiosk.SetPrivateProperty(nameof(Kiosk.DateAdded), entity.DateAdded);
        kiosk.SetPrivateProperty(nameof(Kiosk.AddedBy), entity.AddedBy);
        kiosk.SetPrivateProperty(nameof(Kiosk.LastUpdate), entity.LastUpdate);
        kiosk.SetPrivateProperty(nameof(Kiosk.UpdatedBy), entity.UpdatedBy);

        return kiosk;
    }

    public static KioskModel MapToModel(this Kiosk entity)
    {
        return new KioskModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Address = entity.Address,
            DateAdded = entity.DateAdded,
            AddedBy = entity.AddedBy,
            LastUpdate = entity.LastUpdate,
            UpdatedBy = entity.UpdatedBy
        };
    }
}