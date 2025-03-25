using ControleLancamentos.Domain.Enums;
using MediatR;

namespace ControleLancamentos.Domain.Events.ControleLancamentos
{
    public class LancamentoCreatedEvent : INotification
    {
        public Guid EventId { get; }
        public string NumeroContaBancaria { get; }
        public string AgenciaContaBancaria { get; }
        public TipoTransacao Tipo { get; }
        public decimal Valor { get; }
        public DateTime Data { get; }        

        public LancamentoCreatedEvent(Guid eventId, 
                                      string numeroContaBancaria, 
                                      string agenciaContaBancaria, 
                                      TipoTransacao tipo, 
                                      decimal valor, 
                                      DateTime data, 
                                      CancellationToken cancellationToken = default)
        {
            EventId = eventId;
            NumeroContaBancaria = numeroContaBancaria;
            AgenciaContaBancaria = agenciaContaBancaria;
            Tipo = tipo;
            Valor = valor;
            Data = data;            
        }
    }
}
