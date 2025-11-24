using Microsoft.EntityFrameworkCore;

namespace FastTrack.Repository.Extensions;

public static class QueryableTrackingExtensions
{
    public static IQueryable<T> WithTracking<T>(this IQueryable<T> query, bool trackChanges) where T : class
    {
        return trackChanges ? query : query.AsNoTracking();
    }
}