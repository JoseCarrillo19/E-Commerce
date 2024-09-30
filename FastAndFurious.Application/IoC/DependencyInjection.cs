using E_Commerce.Application.IServices;
using E_Commerce.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Application.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
