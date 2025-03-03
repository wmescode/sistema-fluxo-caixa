using ControleLancamentos.Domain.Repositories;
using ControleLancamentos.ORM;
using ControleLancamentos.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ControleLancamentos.IoC
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

            services.AddScoped<IContaBancariaRepository, ContaBancariaRepository>();

            return services;
        }
    }
}
