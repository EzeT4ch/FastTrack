using FastTrack.Application.Services.Kiosk;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Cores.Grid;
using Shared.Cores.Response.Kiosk;

namespace FastTrack.Api.Endpoints.Kiosk
{
    public class GetKiosksEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/kiosks", HandleAsync)
                .WithTags("Kiosks").WithName("GetKiosks");
        }
        private static async Task<Results<Ok<GridResponse<KioskResponse>>, NotFound<string>>> HandleAsync(GetKiosksService getKiosksService, CancellationToken cToken, [FromBody] GridRequest request)
        {
            GridResponse<KioskResponse> result = await getKiosksService.Execute(request, cToken);

            if (result.TotalRecords == 0)
            {
                return TypedResults.NotFound("No products found.");
            }

            return TypedResults.Ok(result);
        }

    }
}
