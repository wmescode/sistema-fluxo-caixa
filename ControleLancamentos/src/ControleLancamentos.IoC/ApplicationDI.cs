using ControleLancamentos.Application;
using ControleLancamentos.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace ControleLancamentos.IoC
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

            return services;
        }
    }
}
