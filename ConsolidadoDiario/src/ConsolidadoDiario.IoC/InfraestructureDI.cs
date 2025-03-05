using ConsolidadoDiario.Domain.Repositories;
using ConsolidadoDiario.ORM;
using ConsolidadoDiario.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ConsolidadoDiario.IoC
{
    public static class InfraestructureDI
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgreSql");
            services.AddDbContext<DefaultContext>(options =>
            {
                options.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly(typeof(DefaultContext).Assembly.FullName)
                    ).LogTo(Console.WriteLine, LogLevel.Information);
            }, ServiceLifetime.Transient);

            var redisConnectionString = configuration.GetConnectionString("Redis");
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "ConsolidadoDiarioCache";
            });

            services.AddScoped<IConsolidadoDiarioRepository, ConsolidadoDiarioRepository>();            
            return services;
        }
    }
}
