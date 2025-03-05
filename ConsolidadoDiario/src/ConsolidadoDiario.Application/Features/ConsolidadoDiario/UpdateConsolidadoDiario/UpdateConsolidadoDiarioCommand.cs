using ConsolidadoDiario.Domain.Enums;
using MediatR;

namespace ConsolidadoDiario.Application.Features.ConsolidadoDiario.UpdateConsolidadoDiario
{
    public class UpdateConsolidadoDiarioCommand : IRequest
    {
        public string NumeroContaBancaria { get; set; } = string.Empty;
        public string AgenciaContaBancaria { get; set; } = string.Empty;
        public TipoTransacao Tipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
}
