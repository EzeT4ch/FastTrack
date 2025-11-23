using FastTrack.Application.Services.Product;
using FastTrack.Application.Services.Purchase;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastTrack.Application.Extensions
{
    public static class IoC
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ReceivePurchaseOrder>();
            services.AddScoped<GetProductService>();
        }
    }
}
