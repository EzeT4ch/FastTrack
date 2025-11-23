using FastTrack.Persistence.Extensions;
using FastTrack.Persistence.Models;
using FastTrack.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Result;
using Shared.Cores.Grid;
using Shared.Cores.Response.Products;

namespace FastTrack.Application.Services.Product
{
    public class GetProductService(
        IRepository<ProductModel, Core.Entities.Product> productsRepository)
    {
        public async Task<GridResponse<ProductResponse>> GetProductAsync(int? kioskId, GridRequest request, CancellationToken cToken)
        {
            IQueryable<ProductModel> query = productsRepository.AsQueryable().AsNoTracking();

            if (kioskId.HasValue)
            {
                query = query.Where(p => p.KioskId == kioskId.Value);
            }

            if(request.Filters.Count > 0)
            {
                query = query.ApplyFilters(request.Filters);
            }

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.OrderBy(request.SortColumn, request.SortDirection);
            }

            int totalRecords = await query.CountAsync(cToken);
            ProductResponse[] products = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(p => new ProductResponse
                    {
                        Id = p.Id,
                        KioskId = p.KioskId,
                        Name = p.Name,
                        SkuCode = p.Sku,
                        Status = p.Status,
                        DateAdded = p.DateAdded,
                        AddedBy = p.AddedBy,
                        LastUpdate = p.LastUpdate,
                        UpdatedBy = p.UpdatedBy
                    })
                    .ToArrayAsync(cToken);

            return new GridResponse<ProductResponse>(totalRecords, products);
        }
    }
}
