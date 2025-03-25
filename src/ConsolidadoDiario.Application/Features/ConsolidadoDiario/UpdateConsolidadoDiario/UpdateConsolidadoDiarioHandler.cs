using ConsolidadoDiario.Domain.Entities;
using ConsolidadoDiario.Domain.Repositories;
using MediatR;

namespace ConsolidadoDiario.Application.Features.ConsolidadoDiario.UpdateConsolidadoDiario
{
    public class UpdateConsolidadoDiarioHandler : IRequestHandler<UpdateConsolidadoDiarioCommand>
    {
        private readonly IConsolidadoDiarioRepository _consolidadoDiarioRepository;        

        public UpdateConsolidadoDiarioHandler(IConsolidadoDiarioRepository consolidadoDiarioRepository)
        {
            _consolidadoDiarioRepository = consolidadoDiarioRepository;            
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
        }
    }
}
