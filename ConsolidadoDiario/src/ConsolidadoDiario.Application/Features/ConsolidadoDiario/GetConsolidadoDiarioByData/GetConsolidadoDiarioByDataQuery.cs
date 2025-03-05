using MediatR;

namespace ConsolidadoDiario.Application.Features.ConsolidadoDiario.GetConsolidadoDiarioByData
{
    public class GetConsolidadoDiarioByDataQuery : IRequest<GetConsolidadoDiarioByDataResult>
    {
        public string NumeroContaBancaria { get; set; }
        public string AgenciaContaBancaria { get; set; }
        public DateTime Data { get; set; }
    }
}
