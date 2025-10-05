using FastTrack.Core.Exceptions;
using Shared.Abstractions.Interfaces;

namespace FastTrack.Core.Entities
{
    public class OrderDetail : IEntity, ICreatedAuditable, IUpdateAuditable
    {
        public int Id { get; private set; }
        public int Line { get; private set; }
        public string SkuCode { get; private set; }
        public int PurchaseOrderId { get; private set; }
        public int ProductId { get; private set; }
        public DateTime DateAdded { get; private set; }
        public int AddedBy { get; private set; }
        public DateTime LastUpdate { get; private set; }
        public int UpdatedBy { get; private set; }

        private OrderDetail() { }

        private OrderDetail(int line, string skuCode, int purchaseOrderId, int productId, int userId)
        {
            Line = line;
            SkuCode = skuCode;
            PurchaseOrderId = purchaseOrderId;
            ProductId = productId;
            DateAdded = DateTime.UtcNow;
            AddedBy = userId;
            LastUpdate = DateAdded;
            UpdatedBy = userId;
        }

        public static OrderDetail Create(int line, string skuCode, int purchaseOrderId, int productId, int userId)
        {
            if (line <= 0)
                throw new DomainException("La línea de orden debe ser mayor que cero.", nameof(line));

            if (string.IsNullOrWhiteSpace(skuCode))
                throw new DomainException("El código SKU no puede estar vacío.", nameof(skuCode));

            if (purchaseOrderId <= 0)
                throw new DomainException("Debe especificarse una orden de compra válida.", nameof(purchaseOrderId));

            if (productId <= 0)
                throw new DomainException("Debe especificarse un producto válido.", nameof(productId));

            if (userId <= 0)
                throw new DomainException("El usuario creador debe ser válido.", nameof(userId));

            return new OrderDetail(line, skuCode.Trim(), purchaseOrderId, productId, userId);
        }
    }
}
