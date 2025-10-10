using FastTrack.Core.Exceptions;
using Shared.Abstractions.Interfaces;

namespace FastTrack.Core.Entities;

public class Kiosk : IEntity, ICreatedAuditable, IUpdateAuditable
{
    private Kiosk()
    {
    }

    private Kiosk(string name, string email, string address, int userId)
    {
        Name = name;
        Email = email;
        Address = address;
        DateAdded = DateTime.UtcNow;
        AddedBy = userId;
        LastUpdate = DateAdded;
        UpdatedBy = userId;
    }

    public string Name { get; private set; }

    public string Email { get; private set; }

    public string Address { get; private set; }

    public DateTime DateAdded { get; }

    public int AddedBy { get; }
    public int Id { get; private set; }

    public DateTime LastUpdate { get; }

    public int UpdatedBy { get; }

    public static Kiosk Create(string name, string email, string address, int userId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("El nombre del kiosco no puede estar vacío.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("El correo electrónico no puede estar vacío.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            throw new DomainException("La dirección no puede estar vacía.", nameof(address));
        }

        if (userId <= 0)
        {
            throw new DomainException("El usuario creador debe ser válido.", nameof(userId));
        }

        return new Kiosk(name.Trim(), email.Trim(), address.Trim(), userId);
    }
}