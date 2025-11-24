using FastTrack.Application.Services.Kiosk;
using FastTrack.Application.Services.Product;
using FastTrack.Application.Services.Purchase;
using Microsoft.Extensions.DependencyInjection;

namespace FastTrack.Application.Extensions
{
    public static class IoC
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ReceivePurchaseOrder>();
            services.AddScoped<GetProductService>();
            services.AddScoped<GetKiosksService>();
        }
    }
}
