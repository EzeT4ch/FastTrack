using FastTrack.Application.Services.Product;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Result;
using Shared.Cores.Grid;
using Shared.Cores.Response.Products;

namespace FastTrack.Api.Endpoints.Inventory
{
    public class GetProductsEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/products", HandleAsync)
                .WithName("GetProducts")
                .WithTags("Products");
        }

        private static async Task<Results<Ok<GridResponse<ProductResponse>>, NotFound<string>>> HandleAsync(GetProductService service, CancellationToken cToken, [FromBody] GridRequest request, int? kioskId = null)
        {
            GridResponse<ProductResponse> result = await service.GetProductAsync(kioskId, request, cToken);

            if(result.TotalRecords == 0)
            {
                return TypedResults.NotFound("No products found.");
            }

            return TypedResults.Ok(result);
        }
    }
}
