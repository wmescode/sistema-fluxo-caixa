using ControleLancamentos.Domain.Events.ControleLancamentos;
using MediatR;
using StackExchange.Redis;
using System.Globalization;
using System.Text.Json;

namespace ControleLancamentos.Messaging.EventHandler
{
    public class ConsolidadoDiarioEventHandler : INotificationHandler<LancamentoCreatedEvent>
    {
        private readonly IConnectionMultiplexer _redis;
        public ConsolidadoDiarioEventHandler(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task Handle(LancamentoCreatedEvent notification, CancellationToken cancellationToken)
        {
            var db = _redis.GetDatabase();
            
            var eventoJson = JsonSerializer.Serialize(notification);

            await db.StreamAddAsync("lancamentos-consolidado-diario", new NameValueEntry[]
            {
                new NameValueEntry("eventId", notification.EventId.ToString()),
                new NameValueEntry("numeroContaBancaria", notification.NumeroContaBancaria),
                new NameValueEntry("agenciaContaBancaria", notification.AgenciaContaBancaria),
                new NameValueEntry("valor", notification.Valor.ToString(CultureInfo.InvariantCulture)),
                new NameValueEntry("data", notification.Data.ToString("o")), 
                new NameValueEntry("tipo", notification.Tipo.ToString()),
                new NameValueEntry("eventoJson", eventoJson)
            });
        }
    }
}
