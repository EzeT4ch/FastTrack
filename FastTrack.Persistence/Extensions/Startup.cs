using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FastTrack.Persistence.Extensions;

public static class Startup
{
    public static void AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FastTrackDbContext>(options =>
            options.UseSqlServer(connectionString));

    }
}