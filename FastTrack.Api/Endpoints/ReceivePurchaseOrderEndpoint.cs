using FastTrack.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Common.Result;

namespace FastTrack.Api.Endpoints;

public class ReceivePurchaseOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/purchaseorders/{orderId:int}/receive", HandleAsync)
            .WithName("ReceivePurchaseOrder")
            .WithTags("PurchaseOrders")
            .WithOpenApi();
    }
    
    private static async Task<Results<Ok<string>, NotFound<string>, Conflict<string>, BadRequest<string>>> HandleAsync(
        int orderId,
        int userId,
        ReceivePurchaseOrder service,
        CancellationToken ct)
    {
        Result<string, string> result = await service.ReceiveAsync(orderId, userId, ct);

        if (!result.IsFailure)
        {
            return TypedResults.Ok(result.Data);
        }

        string? error = result.Error;

        if (error != null && error.Contains("no encontrada", StringComparison.OrdinalIgnoreCase))
        {
            return TypedResults.NotFound(error);
        }

        if (error != null && error.Contains("ya fue recibida", StringComparison.OrdinalIgnoreCase))
        {
            return TypedResults.Conflict(error);
        }

        return TypedResults.BadRequest(error);

    }
}