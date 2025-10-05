using FastTrack.Core.Exceptions;
using Shared.Abstractions.Interfaces;

namespace FastTrack.Core.Entities
{
    public class CurrentInventory : IEntity, ICreatedAuditable, IUpdateAuditable
    {
        public int Id { get; private set; }

        public int Quantity { get; private set; }

        public int ProductId { get; private set; }

        public int KioskId { get; private set; }

        public DateTime DateAdded { get; private set; }

        public int AddedBy { get; private set; }

        public DateTime LastUpdate { get; private set; }

        public int UpdatedBy { get; private set; }

        private CurrentInventory()
        {

        }
        
        private CurrentInventory(int quantity, int productId, int kioskId, int addedBy)
        {
            Quantity = quantity;
            ProductId = productId;
            KioskId = kioskId;
            DateAdded = DateTime.UtcNow;
            AddedBy = addedBy;
            LastUpdate = DateAdded;
            UpdatedBy = addedBy;
        }
        
        public static CurrentInventory Create(int quantity, int productId, int kioskId, int addedBy)
        {
            if(quantity <= 0)
                throw new DomainException("La cantidad debe ser mayor que cero.", nameof(quantity));
            if(productId <= 0)
                throw new DomainException("Debe especificarse un producto válido.", nameof(productId));
            if(kioskId <= 0)
                throw new DomainException("Debe especificarse un kiosco válido.", nameof(kioskId));
            if(addedBy <= 0)
                throw new DomainException("El usuario creador debe ser válido.", nameof(addedBy));
            
            return new CurrentInventory(quantity, productId, kioskId, addedBy);
        }
    }
}
