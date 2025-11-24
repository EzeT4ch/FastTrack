using FastTrack.Persistence.Extensions;
using FastTrack.Persistence.Models;
using FastTrack.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Cores.Grid;
using Shared.Cores.Response.Kiosk;

namespace FastTrack.Application.Services.Kiosk
{
    public class GetKiosksService(IRepository<KioskModel, Core.Entities.Kiosk> kioskRepository)
    {
        public async Task<GridResponse<KioskResponse>> Execute(GridRequest request, CancellationToken cToken)
        {
            IQueryable<KioskModel> query = kioskRepository.AsQueryable().AsNoTracking();

            if (request.Filters.Count > 0)
            {
                query = query.ApplyFilters(request.Filters);
            }

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.OrderBy(request.SortColumn, request.SortDirection);
            }

            int totalRecords = await query.CountAsync(cToken);
            KioskResponse[] kiosks = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(p => new KioskResponse
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Code = p.Code,
                        Email = p.Email,
                        Address = p.Address,
                        DateAdded = p.DateAdded,
                        AddedBy = p.AddedBy,
                        LastUpdate = p.LastUpdate,
                        UpdatedBy = p.UpdatedBy
                    })
                    .ToArrayAsync(cToken);

            return new GridResponse<KioskResponse>(totalRecords, kiosks);
        }
    }
}
