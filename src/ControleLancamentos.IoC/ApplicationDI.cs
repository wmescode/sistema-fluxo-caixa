using ControleLancamentos.Application;
using ControleLancamentos.Application.PipelineBehavior;
using ControleLancamentos.Messaging;
using MediatR;
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
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            return services;
        }
    }
}
