using ConsolidadoDiario.Application;
using ConsolidadoDiario.Messaging;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using ConsolidadoDiario.Application.PipelineBehavior;

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

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ClearCacheBehavior<,>));

            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
            services.AddValidatorsFromAssembly(typeof(MessagingLayer).Assembly);                       
            services.AddHostedService<RedisStreamsBackgroundService>();            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());            

            return services;
        }
    }
}
