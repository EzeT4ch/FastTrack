using FastTrack.Application.Services.Product;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Cores.Request;

namespace FastTrack.Api.Endpoints.Inventory
{
    public class CreateProductEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/products/create", HandleAsync)
                .WithName("CreateProduct")
                .WithTags("Products");
        }

        private async Task<Results<Ok<string>, BadRequest<string>>> HandleAsync([FromBody] CreateProductRequest request, CreateProductService createProductService, CancellationToken cToken)
        {
            var result = await createProductService.ExecuteAsync(request);

            if (result.IsFailure)
                return TypedResults.BadRequest(result.Error);

            return TypedResults.Ok(result.Data);
        }
    }
}
