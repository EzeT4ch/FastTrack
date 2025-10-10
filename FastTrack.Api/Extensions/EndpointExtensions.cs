using System.Reflection;
using FastTrack.Api.Endpoints;

namespace FastTrack.Api.Extensions;

public static class EndpointExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        IEnumerable<IEndpoint> endpoints = Assembly.GetExecutingAssembly()
            .ExportedTypes
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false })
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>();

        foreach (IEndpoint endpoint in endpoints)
            endpoint.MapEndpoint(app);
    }
}