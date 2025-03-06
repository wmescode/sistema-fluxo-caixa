using Microsoft.Extensions.Configuration;
using Elastic.Apm.SerilogEnricher;
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.DependencyInjection;

namespace ControleLancamentos.IoC
{
    public static class LoggingConfigDI
    {
        public static void AddLogConfig(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
               .MinimumLevel.Override("System", LogEventLevel.Error)
               .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
               .Enrich.WithProperty("ServiceName", configuration.GetSection("ElasticApm:ServiceName").Value)
               .Enrich.WithProperty("Application", configuration.GetSection("ElasticApm:ServiceName").Value)
               .Enrich.FromLogContext()
               .Enrich.WithElasticApmCorrelationInfo()
               .WriteTo.Elasticsearch(new[] { new Uri(configuration.GetSection("ElasticSearch:Url").Value) }, opts =>
               {
                   opts.DataStream = new DataStreamName("Logs", configuration.GetSection("ElasticApm:ServiceName").Value);
                   opts.BootstrapMethod = BootstrapMethod.Failure;
                   opts.ConfigureChannel = channelOpts =>
                   {
                       channelOpts.BufferOptions = new BufferOptions
                       {

                       };
                   };
               })
               .CreateLogger();
        }

        public static IServiceCollection AddObservability(this IServiceCollection services)
        {
            services.AddAllElasticApm();
            services.AddLogging(lb => lb.AddSerilog());
            services.AddElasticApm();

            services.AddElasticApmForAspNetCore(
                new Elastic.Apm.DiagnosticSource.IDiagnosticsSubscriber[]
                {
                    new Elastic.Apm.AspNetCore.DiagnosticListener.AspNetCoreDiagnosticSubscriber(),
                    new Elastic.Apm.EntityFrameworkCore.EfCoreDiagnosticsSubscriber(),
                }
            );

            return services;
        }
    }

}
