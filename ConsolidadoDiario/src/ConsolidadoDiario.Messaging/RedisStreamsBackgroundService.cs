using ConsolidadoDiario.Application.Features.ConsolidadoDiario.UpdateConsolidadoDiario;
using ConsolidadoDiario.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Globalization;

namespace ConsolidadoDiario.Messaging
{
    public class RedisStreamsBackgroundService : BackgroundService
    {
        private readonly IConnectionMultiplexer _redis;       
        private readonly ILogger<RedisStreamsBackgroundService> _logger;
        private readonly string _streamName;
        private readonly string _consumerGroup;
        private readonly string _consumerName;
        private readonly IServiceProvider _serviceProvider;


        public RedisStreamsBackgroundService(IConnectionMultiplexer redis,
                                             ILogger<RedisStreamsBackgroundService> logger,
                                             IServiceProvider serviceProvider,
                                             IConfiguration configuration)
        {
            _redis = redis ?? throw new ArgumentNullException(nameof(redis));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _streamName = configuration["Redis:StreamName"] ?? throw new ArgumentNullException("Redis:StreamName");
            _consumerGroup = configuration["Redis:ConsumerGroup"] ?? throw new ArgumentNullException("Redis:ConsumerGroup");
            _consumerName = configuration["Redis:ConsumerName"] ?? throw new ArgumentNullException("Redis:ConsumerName");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var db = _redis.GetDatabase();

            // Garante que o grupo existe
            await CreateConsumerGroupIfNotExistsAsync(db);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Lê mensagens do stream
                    var messages = await db.StreamReadGroupAsync(
                        _streamName,
                        _consumerGroup,
                        _consumerName,
                        ">", // Lê novas mensagens não entregues
                        count: 5 
                    );

                    if (messages.Any())
                    {
                        foreach (var message in messages)
                        {
                            try
                            {                                
                                var integrationEvent = DeserializeMessage(message);
                                if(integrationEvent != null)
                                {
                                    using (var scope = _serviceProvider.CreateScope())
                                    {
                                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                                        await mediator.Send(integrationEvent, stoppingToken);
                                    }

                                }
                                // Confirma o processamento (ACK)
                                await db.StreamAcknowledgeAsync(_streamName, _consumerGroup, message.Id);
                                _logger.LogInformation("Evento {EventId} processado com sucesso", message.Id);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Erro ao processar mensagem {MessageId}", message.Id);
                            }
                        }
                    }
                    else
                    {
                        await Task.Delay(1000, stoppingToken); 
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro na leitura do stream");
                    await Task.Delay(5000, stoppingToken); 
                }
            }
        }
        private async Task CreateConsumerGroupIfNotExistsAsync(IDatabase db)
        {
            try
            {
                await db.StreamCreateConsumerGroupAsync(_streamName, _consumerGroup, "0-0", true);
            }
            catch (RedisException ex) when (ex.Message.Contains("BUSYGROUP")){}
        }

        private UpdateConsolidadoDiarioCommand? DeserializeMessage(StreamEntry message)
        {
            try
            {
                var numeroContaBancaria = message["numeroContaBancaria"].ToString();
                var agenciaContaBancaria = message["agenciaContaBancaria"].ToString();
                var valor = decimal.Parse(message["valor"], CultureInfo.InvariantCulture);
                var data = DateTime.Parse(message["data"], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);                
                var tipo = Enum.Parse<TipoTransacao>(message["tipo"]);

                return new UpdateConsolidadoDiarioCommand
                {
                    NumeroContaBancaria = numeroContaBancaria,
                    AgenciaContaBancaria = agenciaContaBancaria,
                    Valor = valor,
                    Data = data,
                    Tipo = tipo
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desserializar a mensagem {MessageId}", message.Id);
                return null;
            }
        }        
    }
}
