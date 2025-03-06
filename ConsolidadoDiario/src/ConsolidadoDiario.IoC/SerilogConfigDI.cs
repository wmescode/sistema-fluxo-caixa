using Elastic.Apm.SerilogEnricher;
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;


namespace ConsolidadoDiario.IoC
{
    public static class DISerilogExtensions
    {
        public static void AddLogConfig(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .MinimumLevel.Override("System", LogEventLevel.Warning)
               .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
               .Enrich.WithProperty("ServiceName", configuration.GetSection("Redis:ConsumerName").Value)
               .Enrich.WithProperty("Application", configuration.GetSection("ElasticApm:ServiceName").Value)
               .Enrich.FromLogContext()
               .Enrich.WithElasticApmCorrelationInfo()
               .WriteTo.Elasticsearch(new[] { new Uri(configuration.GetSection("ElasticSearch:Url").Value) }, opts =>
               {
                   opts.DataStream = new DataStreamName("Logs", configuration.GetSection("Redis:ConsumerName").Value);
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
    }
}
