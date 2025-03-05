using ConsolidadoDiario.Application;
using ConsolidadoDiario.Messaging;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

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

            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
            services.AddValidatorsFromAssembly(typeof(MessagingLayer).Assembly);                       
            services.AddHostedService<RedisStreamsBackgroundService>();            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());            

            return services;
        }
    }
}
