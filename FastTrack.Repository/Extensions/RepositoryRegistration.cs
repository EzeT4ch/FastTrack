using FastTrack.Persistence;
using FastTrack.Repository.Interfaces;
using FastTrack.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FastTrack.Repository.Extensions;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepository<TModel, TDomain>(
        this IServiceCollection services,
        Func<TModel, TDomain> toDomain,
        Func<TDomain, TModel> toModel)
        where TModel : class
        where TDomain : class
    {
        services.AddScoped<IRepository<TModel, TDomain>>(sp =>
        {
            FastTrackDbContext context = sp.GetRequiredService<FastTrackDbContext>();
            return new Repository<TModel, TDomain>(context, toDomain, toModel);
        });

        return services;
    }
}