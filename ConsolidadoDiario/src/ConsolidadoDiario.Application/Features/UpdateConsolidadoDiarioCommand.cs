using ConsolidadoDiario.Domain.Enums;
using MediatR;

namespace ConsolidadoDiario.Application.Features
{
    public class UpdateConsolidadoDiarioCommand : IRequest
    {
        public string NumeroContaBancaria { get; set; }
        public string AgenciaContaBancaria { get; set; }
        public TipoTransacao Tipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
}
