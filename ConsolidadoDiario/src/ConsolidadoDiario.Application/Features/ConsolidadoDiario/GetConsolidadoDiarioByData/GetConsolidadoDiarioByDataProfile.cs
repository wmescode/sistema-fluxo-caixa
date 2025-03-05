using AutoMapper;
using ConsolidadoDiario.Domain.Entities;

namespace ConsolidadoDiario.Application.Features.ConsolidadoDiario.GetConsolidadoDiarioByData
{
    public class GetConsolidadoDiarioByDataProfile : Profile
    {
        public GetConsolidadoDiarioByDataProfile()
        {
            CreateMap<ConsolidadoDiarioConta, GetConsolidadoDiarioByDataResult>();
        }
    }
}
