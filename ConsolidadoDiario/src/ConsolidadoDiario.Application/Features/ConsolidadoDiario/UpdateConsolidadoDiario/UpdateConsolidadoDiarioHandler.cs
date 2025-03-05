using ConsolidadoDiario.Domain.Entities;
using ConsolidadoDiario.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace ConsolidadoDiario.Application.Features.ConsolidadoDiario.UpdateConsolidadoDiario
{
    public class UpdateConsolidadoDiarioHandler : IRequestHandler<UpdateConsolidadoDiarioCommand>
    {
        private readonly IConsolidadoDiarioRepository _consolidadoDiarioRepository;
        private readonly IDistributedCache _cache;       

        public UpdateConsolidadoDiarioHandler(IConsolidadoDiarioRepository consolidadoDiarioRepository,
                                              IDistributedCache cache)
        {
            _consolidadoDiarioRepository = consolidadoDiarioRepository;
            _cache = cache;            
        }

        public async Task Handle(UpdateConsolidadoDiarioCommand request, CancellationToken cancellationToken)
        {            
            var consolidadoDiario = await _consolidadoDiarioRepository.GetConsolidadoDiarioContaAsync(request.NumeroContaBancaria, request.AgenciaContaBancaria, request.Data, cancellationToken);

            if(consolidadoDiario == null)
            {                
                consolidadoDiario = new ConsolidadoDiarioConta(request.NumeroContaBancaria, request.AgenciaContaBancaria, request.Data);
                consolidadoDiario.AtualizaSaldoConsolidado(request.Tipo, request.Valor);
                await _consolidadoDiarioRepository.AddConsolidadoDiarioContaAsync(consolidadoDiario, cancellationToken);
            }
            else
            {
                consolidadoDiario.AtualizaSaldoConsolidado(request.Tipo, request.Valor);
                await _consolidadoDiarioRepository.UpdateConsolidadoDiarioContaAsync(consolidadoDiario, cancellationToken);
            }

            // Limpa o cache após atualização
            var cacheKey = $"ConsolidadoDiario_{request.NumeroContaBancaria}_{request.AgenciaContaBancaria}_{request.Data:yyyy-MM-dd}";
            await _cache.RemoveAsync(cacheKey, cancellationToken);
        }
    }
}
