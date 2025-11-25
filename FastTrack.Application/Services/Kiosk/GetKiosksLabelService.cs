using FastTrack.Persistence.Models;
using FastTrack.Repository.Interfaces;
using Shared.Common.Result;
using Shared.Cores.Request;

namespace FastTrack.Application.Services.Kiosk
{
    public class GetKiosksLabelService(IRepository<KioskModel, Core.Entities.Kiosk> repository)
    {
        public async Task<Result<GetKiosksLabelReponse[], string>> ExecuteAsync(CancellationToken cToken)
            => await repository.GetAllAsync(cToken)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted || task.Result is null)
                    {
                        return Result<GetKiosksLabelReponse[], string>.SetError("Failed to retrieve kiosks.");
                    }
                    GetKiosksLabelReponse[] kiosks = task.Result.Select(kiosk => new GetKiosksLabelReponse
                    {
                        KioskId = kiosk.Id,
                        Name = kiosk.Name
                    }).ToArray();
                    return Result<GetKiosksLabelReponse[], string>.SetSuccess(kiosks);
                }, cToken);
    }
}
