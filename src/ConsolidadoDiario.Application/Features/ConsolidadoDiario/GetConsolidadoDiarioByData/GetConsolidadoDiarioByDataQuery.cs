using ConsolidadoDiario.Application.PipelineBehavior;
using MediatR;

namespace ConsolidadoDiario.Application.Features.ConsolidadoDiario.GetConsolidadoDiarioByData
{
    public class GetConsolidadoDiarioByDataQuery : IRequest<GetConsolidadoDiarioByDataResult>, ICacheable
    {
        public string NumeroContaBancaria { get; set; }
        public string AgenciaContaBancaria { get; set; }
        public DateTime Data { get; set; }
        public string CacheKey => $"ConsolidadoDiario_{NumeroContaBancaria}_{AgenciaContaBancaria}_{Data:yyyy-MM-dd}";
        public int CacheDurationMinutes => 60; 
    }
}
