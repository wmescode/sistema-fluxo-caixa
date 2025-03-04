using ConsolidadoDiario.Application.Features;
using ConsolidadoDiario.Domain.Enums;
using MediatR;
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
        //private readonly IMediator _mediator;
        private readonly ILogger<RedisStreamsBackgroundService> _logger;
        private readonly string _streamName = "lancamentos-consolidado-diario";
        private readonly string _consumerGroup = "consolidado-diario-group";
        private readonly string _consumerName = "consumer-1";
        private readonly IServiceProvider _serviceProvider;


        public RedisStreamsBackgroundService(IConnectionMultiplexer redis, 
                                             //IMediator mediator, 
                                             ILogger<RedisStreamsBackgroundService> logger,
                                             IServiceProvider serviceProvider)
        {
            _redis = redis;
            //_mediator = mediator;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var db = _redis.GetDatabase();

            // Garante que o grupo existe (opcional, pode ser feito manualmente)
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
                        count: 5 // Ajustar conforme necessidade
                    );

                    if (messages.Any())
                    {
                        foreach (var message in messages)
                        {
                            try
                            {                                
                                var integrationEvent = DeserializeMessage(message);

                                //await _mediator.Send(integrationEvent);                                
                                using (var scope = _serviceProvider.CreateScope())
                                {
                                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                                    await mediator.Send(integrationEvent, stoppingToken);
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
            catch (RedisException ex) when (ex.Message.Contains("BUSYGROUP"))
            {
                // Grupo já existe, não precisa fazer nada
            }
        }

        private UpdateConsolidadoDiarioCommand DeserializeMessage(StreamEntry message)
        {
            // Desserializa usando os campos individuais            
            string numeroContaBancaria = message["numeroContaBancaria"].ToString();
            string agenciaContaBancaria = message["agenciaContaBancaria"].ToString();
            decimal valor = decimal.Parse(message["valor"], CultureInfo.InvariantCulture);
            DateTime data = DateTime.Parse(message["data"], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            TipoTransacao tipo = Enum.Parse<TipoTransacao>(message["tipo"]);

            return new UpdateConsolidadoDiarioCommand { NumeroContaBancaria = numeroContaBancaria, AgenciaContaBancaria = agenciaContaBancaria, Valor = valor, Data = data, Tipo = tipo };         
        }        
    }
}
