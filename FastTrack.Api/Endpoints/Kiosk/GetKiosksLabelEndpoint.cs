using FastTrack.Application.Services.Kiosk;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Common.Result;
using Shared.Cores.Request;

namespace FastTrack.Api.Endpoints.Kiosk
{
    public class GetKiosksLabelEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/kiosks/labels", HandleAsync)
                .WithName("GetKiosksLabels")
                .WithTags("Kiosks");
        }

        private async Task<Results<Ok<GetKiosksLabelReponse[]>, NotFound<string>>> HandleAsync(GetKiosksLabelService getKiosksLabelService, CancellationToken cToken)
        {
            Result<GetKiosksLabelReponse[], string> result = await getKiosksLabelService.ExecuteAsync(cToken);
            if (result.IsFailure)
                return TypedResults.NotFound(result.Error);

            return TypedResults.Ok(result.Data);
        }
    }
}
