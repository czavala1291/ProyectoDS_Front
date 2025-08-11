using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Venta.Infrastructure.External
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddExternalServiceVentaAPI(configuration);
            return services;
        }
        private static IServiceCollection AddExternalServiceVentaAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("VentaAPI", client =>
            {
                client.BaseAddress = new Uri(configuration["VentaAPI:BaseUrl"] ?? "https://localhost:5001/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return services;
        }
    }
}
