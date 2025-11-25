using FastTrack.Core.Entities;
using FastTrack.Persistence.Models;
using FastTrack.Repository.Interfaces;
using Shared.Common.Result;
using Shared.Cores.Request;

namespace FastTrack.Application.Services.Product
{
    public class CreateProductService(IRepository<ProductModel, Core.Entities.Product> productRepository,
        IRepository<KioskModel, Core.Entities.Kiosk> kioskRepository,
        IRepository<CurrentInventoryModel, CurrentInventory> currentInventoryRepository,
        IRepository<InventoryMovementModel, InventoryMovement> inventoryMovementRepository,
        IUnitOfWork unitOfWork)
    {
        public async Task<Result<string, string>> ExecuteAsync(CreateProductRequest request)
        {
            Result<string, string>? validateProduct = await ValidateProduct(request);
            if (validateProduct is not null)
                return validateProduct;


            return Result<string, string>.SetSuccess("Producto creado exitosamente");
        }

        private async Task<Result<string, string>>? ValidateProduct(CreateProductRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Name))
                return Result<string, string>.SetError("El nombre del producto es obligatorio");
            
            if(string.IsNullOrEmpty(request.SkuCode))
                return Result<string, string>.SetError("El código SKU es obligatorio");

            if(string.IsNullOrWhiteSpace(request.Description))
                return Result<string, string>.SetError("La descripción del producto es obligatoria");

            if(string.IsNullOrWhiteSpace(request.Price.ToString()) || request.Price < 0)
                return Result<string, string>.SetError("El precio del producto debe ser mayor o igual a cero");

            var acc = await kioskRepository.FindAsync(x => x.Id == request.KioskId);

            if (acc is null)
                return Result<string, string>.SetError("La ubicación no existe");

            var existingProduct = await productRepository.FindAsync(x => x.Sku == request.SkuCode && x.KioskId == request.KioskId);

            if (existingProduct is not null)
                return Result<string, string>.SetError("El código SKU ya existe en la ubicación seleccionada");

            if (request.InitialStock < 0)
                return Result<string, string>.SetError("El stock inicial no puede ser negativo");

            return null;
        }
    }
}
