using ConsolidadoDiario.Application;
using ConsolidadoDiario.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace ConsolidadoDiario.IoC
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplications(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(MessagingLayer).Assembly
                );
            });
            services.AddHostedService<RedisStreamsBackgroundService>();

            return services;
        }
    }
}
